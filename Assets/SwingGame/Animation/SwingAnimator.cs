using System;
using System.Collections;
using System.Collections.Generic;
using SwingGame.Media;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class SwingAnimator : Animator, IMultiplayerInterface
{
    //speed
    public const float VerticalSpeed = 25.0f;
    public const float HorizontalSpeed = 18.0f;

    public enum Orientation
    {
        Vertical,
        Horizontal
    }

    public enum Direction
    {
        DirectionRight,
        DirectionLeft,
        DirectionUp,
        DirectionDown
    }

    public enum StateAnim
    {
        StateUpDeplBall,
        StateSideDeplBall,
        StateDownDeplBall
    }

    private GameZone _gameZone;

    private SwingObject[] _swings;
    private List<FloatingBallContainer> _floatingBallsContainers;
    private bool[][] _occupiedByFloating;
    private int[] _dropingBallByCol;

    public SwingAnimator(GameZone gz, SwingObject[] swings)
    {
        _gameZone = gz;
        _swings = swings;
        int sizeSwings = _swings.Length;
        for (int i = 0; i < sizeSwings; i++)
        {
            _swings[i].Animator = this;
        }

        _floatingBallsContainers = new List<FloatingBallContainer>();

        //Init occupation Tab
        _occupiedByFloating = new bool[GameZone.LengthPlayGround][];
        for (int i = 0; i < GameZone.LengthPlayGround; i++)
        {
            _occupiedByFloating[i] = new bool[GameZone.HeightPlayGround];
            for (int j = 0; j < GameZone.HeightPlayGround; j++)
            {
                _occupiedByFloating[i][j] = false;
            }
        }

        _dropingBallByCol = new int[GameZone.LengthPlayGround];
        for (int i = 0; i < GameZone.LengthPlayGround; i++)
        {
            _dropingBallByCol[i] = 0;
        }
    }

    public void AddFlyingBall(Ball b, float nbUp, float nbSide, Vector2 position, SwingAnimator.Direction dirHorizontal)
    {
        _floatingBallsContainers.Add(new FloatingBallContainer(ball: b,
                state: StateAnim.StateUpDeplBall, position: position, nbSide: nbSide,
                nbUp: nbUp + GameZone.SpacingFlyingBallPlayground, directionSide: dirHorizontal,
                directionUp: Direction.DirectionUp));
    }

    public void AddDropingBall(Ball b, Vector2 position, float nbUp = 0)
    {
        Vector2Int posRounded = new Vector2Int((int)Math.Round(position.x), (int)Math.Round(position.y - 1));
        if (_gameZone.IsPositionFree(posRounded))
        {
            _floatingBallsContainers.Add(
                new FloatingBallContainer(ball: b,
                    state: StateAnim.StateDownDeplBall, position: position, nbSide: 0, nbUp: nbUp, directionSide: Direction.DirectionLeft,
                    directionUp: Direction.DirectionDown));
            _dropingBallByCol[posRounded.x]++;
        }
        else
        {
            Debug.Log("Bad SwingAnimator.AddDropingBall call at position : " + position);
            _gameZone.Playground[(int)Math.Round(position.y)][(int)Math.Round(position.x)].Ball = b;
        }
    }

    public bool CanDropBallAt(int x, int y)
    {
        return !_occupiedByFloating[x][y];
    }

    public int NbDropingBallAtCol(int index)
    {
        return _dropingBallByCol[index];
    }

    public bool Animate(float deltaT)
    {
        bool gameOver = false;
        int sizeFloatingBall = _floatingBallsContainers.Count;
        List<FloatingBallContainer> toRemove = new List<FloatingBallContainer>();

        for (int i = 0; i < sizeFloatingBall; i++)
        {
            FloatingBallContainer floatingBall = _floatingBallsContainers[i];
            if (!floatingBall.Paused)
            {
                floatingBall.Animation.Actualize(deltaT, floatingBall.ContainerObject);
            }

            if (floatingBall.Animation.IsFinished())
            {
                NextAnimation(floatingBall, toRemove);
            }
        }

        int sizeToRemove = toRemove.Count;
        for (int i = 0; i < sizeToRemove; i++)
        {
            FloatingBallContainer floatingBall = toRemove[i];
            int posX = (int)Math.Round(floatingBall.Position.x);
            int posY = (int)Math.Round(floatingBall.Position.y);
            if (GameZone.IsInPlaygroundBounds(new Vector2(posX, posY)))
            {
                AudioManager audioManager = AudioManager.GetInstance();
                audioManager.PlayLandingBallSound(floatingBall.Ball.BallObject);

                _gameZone.Playground[posY][posX].Ball =
                    floatingBall.Ball;
                _occupiedByFloating[posX][posY] = false;
                Destroy(floatingBall.ContainerObject);
                _floatingBallsContainers.Remove(floatingBall);
            }
            else
            {
                floatingBall.Paused = true;
                gameOver = true;
            }

        }

        return gameOver;
    }


    public void NextAnimation(FloatingBallContainer floatingBall, List<FloatingBallContainer> toRemove)
    {
        floatingBall.Paused = false;
        switch (floatingBall.State)
        {
            case StateAnim.StateUpDeplBall:
                ChangeFromUpDepl(floatingBall);
                break;
            case StateAnim.StateSideDeplBall:
                ChangeFromSideDepl(floatingBall);
                break;
            case StateAnim.StateDownDeplBall:
                ChangeFromDownDepl(floatingBall, toRemove);
                break;
        }
    }


    private void ComputeSideDepl(FloatingBallContainer floatingBall, float maxDepl = -1)
    {
        if (maxDepl < 0)
        {
            maxDepl = floatingBall.Direction == Direction.DirectionRight
                ? GameZone.LengthPlayGround - floatingBall.Position.x - 1
                : floatingBall.Position.x;
        }

        float depl;
        float realDepl;
        if (floatingBall.NbSide <= maxDepl)
        {
            depl = floatingBall.NbSide;
            realDepl = depl;
        }
        else
        {
            depl = maxDepl;
            realDepl = depl + (GameZone.DistanceExitFlying) / GameZone.SpacingBall;
        }

        if (floatingBall.State == StateAnim.StateSideDeplBall)
        {
            realDepl += -1 + GameZone.DistanceExitFlying / GameZone.SpacingBall;
        }

        floatingBall.NbSide -= depl;
        floatingBall.Animation.Change(Orientation.Horizontal, floatingBall.Direction, realDepl);
    }

    private void ChangeFromUpDepl(FloatingBallContainer floatingBall)
    {
        ComputeSideDepl(floatingBall);
        floatingBall.Position += new Vector2(0, floatingBall.NbUp);
        floatingBall.State = StateAnim.StateSideDeplBall;
    }

    private void ChangeFromSideDepl(FloatingBallContainer floatingBall)
    {
        if (floatingBall.NbSide > 0)
        {

            //Ball is arrived at the edge of the gamezone so it has to swap to the other side
            ComputeSideDepl(floatingBall, GameZone.LengthPlayGround);
            float depl = (floatingBall.Direction == Direction.DirectionRight ? -1 : 1) *
                         (GameZone.LengthPlayGround * GameZone.SpacingBall + 2f * GameZone.DistanceExitFlying - GameZone.SpacingBall);
            floatingBall.ContainerObject.transform.Translate(new Vector3(depl, 0, 0));

            //send the object to the other player.
            if (MultiplayerSystem.getInstance() != null)
            {
                //move container to center zone
                floatingBall.ContainerObject.transform.Translate(-_gameZone.ZoneGlobal.transform.position.x, 0, 0);
                //create data to send
                MultiplayerData data = new MultiplayerData();
                data.Sender = this;
                data.Data = floatingBall;
                //Remove the floating ball containers from this player
                this._floatingBallsContainers.Remove(floatingBall);
                //send data
                MultiplayerSystem.getInstance().SendData(data);
            }
            else
            {
                floatingBall.TransformBall();
            }
        }
        else
        {
            // Ball is arrived at the column of destination so it has to go down
            int posX = (int)Math.Round((floatingBall.Position.x +
                                         (floatingBall.Direction == Direction.DirectionRight ? 1 : -1) *
                                         floatingBall.NbSideTotal %
                                         GameZone.LengthPlayGround +
                                         GameZone.LengthPlayGround) % GameZone.LengthPlayGround);
            if (CanDropBallAt(posX, GameZone.HeightPlayGround - 1))
            {
                floatingBall.Position =
                    new Vector2(posX, GameZone.HeightPlayGround + GameZone.SpacingFlyingBallPlayground);
                _dropingBallByCol[posX]++;
                floatingBall.State = StateAnim.StateDownDeplBall;
                floatingBall.NbUp = GameZone.SizeBall + GameZone.SpacingFlyingBallPlayground;
                floatingBall.Animation.Change(Orientation.Vertical, Direction.DirectionDown,
                    GameZone.SizeBall + GameZone.SpacingFlyingBallPlayground);
                if (GameZone.IsInPlaygroundBounds(new Vector2Int(posX, GameZone.HeightPlayGround - 1)))
                {
                    ;
                    _occupiedByFloating[posX][GameZone.HeightPlayGround - 1] = true;
                }
            }
            else
            {
                floatingBall.Paused = true;
            }
        }
    }

    private void ChangeFromDownDepl(FloatingBallContainer floatingBall, List<FloatingBallContainer> toRemove)
    {
        floatingBall.Position += new Vector2(0, -floatingBall.NbUp);
        int posX = (int)Math.Round(floatingBall.Position.x);
        int posY = (int)Math.Round(floatingBall.Position.y);

        if (_gameZone.IsPositionFree(new Vector2Int(posX, posY - 1)))
        {
            if (CanDropBallAt(posX, posY - 1))
            {
                //Ball can still go down so it does go down
                if (GameZone.IsInPlaygroundBounds(new Vector2(posX, posY)))
                {
                    _occupiedByFloating[posX][posY] = false;
                }
                floatingBall.NbUp = 1;
                floatingBall.Animation.ResetDepl(1);

                if (GameZone.IsInPlaygroundBounds(new Vector2Int(posX, posY - 1)))
                {
                    _occupiedByFloating[posX][posY - 1] = true;
                }
            }
            else
            {
                floatingBall.NbUp = 0;
                floatingBall.Animation.ResetDepl(0);
                floatingBall.Paused = true;
            }
        }
        else
        {
            //Ball can't go down anymore
            toRemove.Add(floatingBall);

            _dropingBallByCol[posX]--;
        }
    }


    public struct BallAtIndex : IEquatable<BallAtIndex>, IComparable<BallAtIndex>
    {
        private int _index;
        private bool _floating;
        private Ball _ball;

        public BallAtIndex(int index, Ball b, bool floating)
        {
            _index = index;
            _ball = b;
            _floating = floating;
        }

        public Ball Ball
        {
            get => _ball;
        }

        public bool Equals(BallAtIndex other)
        {
            return _index == other._index;
        }

        public int CompareTo(BallAtIndex other)
        {
            return Equals(other) ? (_floating == other._floating ? 0 : (_floating ? -1 : 1)) : (_index < other._index ? -1 : 1);
        }
    }

    public List<BallAtIndex> GetBallsUnderSwingPosition(Vector2Int swingPosition, bool removeFromFloatingBalls = false)
    {
        List<BallAtIndex> listBalls = new List<BallAtIndex>();
        List<FloatingBallContainer> listFloatingBallContainersToCompute = new List<FloatingBallContainer>();

        int nbDropingBallAtCol = _dropingBallByCol[swingPosition.x];
        int sizeFloatingBall = _floatingBallsContainers.Count;
        for (int i = 0; i < sizeFloatingBall; i++)
        {
            FloatingBallContainer floatingBall = _floatingBallsContainers[i];
            int posX = (int)Math.Round(floatingBall.Position.x);
            int posY = (int)Math.Round(floatingBall.Position.y);
            if (floatingBall.State == StateAnim.StateDownDeplBall && posX == swingPosition.x &&
                posY - floatingBall.NbUp <= swingPosition.y + nbDropingBallAtCol)
            {
                if (removeFromFloatingBalls)
                {
                    floatingBall.Position += new Vector2(0, -floatingBall.NbUp);
                    _occupiedByFloating[posX][(int)Math.Round(posY - floatingBall.NbUp)] = false;
                    _dropingBallByCol[posX]--;
                }
                listFloatingBallContainersToCompute.Add(floatingBall);
            }
        }

        int sizeToCompute = listFloatingBallContainersToCompute.Count;
        for (int i = 0; i < sizeToCompute; i++)
        {
            listBalls.Add(new BallAtIndex((int)Math.Round(listFloatingBallContainersToCompute[i].Position.x),
                listFloatingBallContainersToCompute[i].Ball, true));
            if (removeFromFloatingBalls)
            {
                Destroy(listFloatingBallContainersToCompute[i].ContainerObject);
                _floatingBallsContainers.Remove(listFloatingBallContainersToCompute[i]);
            }
        }

        return listBalls;
    }

    public List<Ball> GetBallsBetweenPosition(int positionX, int positionY1, int positionY2, bool removeFromFloatingBalls = false, bool checkForSwings = false)
    {
        if (positionY1 > positionY2) (positionY1, positionY2) = (positionY2, positionY1);

        List<Ball> listBalls = new List<Ball>();
        List<FloatingBallContainer> listFloatingBallContainersToCompute = new List<FloatingBallContainer>();

        int sizeFloatingBall = _floatingBallsContainers.Count;
        for (int i = 0; i < sizeFloatingBall; i++)
        {
            FloatingBallContainer floatingBall = _floatingBallsContainers[i];
            int posX = (int)Math.Round(floatingBall.Position.x);
            int posY = (int)Math.Round(floatingBall.Position.y);
            if (floatingBall.State == StateAnim.StateDownDeplBall && posX == positionX &&
                posY - floatingBall.NbUp >= positionY1 && posY - floatingBall.NbUp <= positionY2)
            {
                if (removeFromFloatingBalls)
                {
                    floatingBall.Position += new Vector2(0, -floatingBall.NbUp);
                    _occupiedByFloating[posX][(int)Math.Round(posY - floatingBall.NbUp)] = false;
                    _dropingBallByCol[posX]--;
                }
                listFloatingBallContainersToCompute.Add(floatingBall);
            }
        }

        int sizeToCompute = listFloatingBallContainersToCompute.Count;
        for (int i = 0; i < sizeToCompute; i++)
        {
            listBalls.Add(listFloatingBallContainersToCompute[i].Ball);
            if (removeFromFloatingBalls)
            {
                Destroy(listFloatingBallContainersToCompute[i].ContainerObject);
                _floatingBallsContainers.Remove(listFloatingBallContainersToCompute[i]);
            }
        }

        return listBalls;
    }


    public void ExplodeFloatingBalls()
    {
        int sizeFloatingBall = _floatingBallsContainers.Count;
        List<FloatingBallContainer> toRemove = new List<FloatingBallContainer>();

        for (int i = 0; i < sizeFloatingBall; i++)
        {
            FloatingBallContainer floatingBall = _floatingBallsContainers[i];
            toRemove.Add(floatingBall);
            floatingBall.ExplodeBall(_gameZone);
        }

        int sizeToRemove = toRemove.Count;
        for (int i = 0; i < sizeToRemove; i++)
        {
            _floatingBallsContainers.Remove(toRemove[i]);
        }
    }

    public void ReceiveData(MultiplayerData data)
    {

        //Move the container to the zone
        data.Data.ContainerObject.transform.Translate(_gameZone.ZoneGlobal.transform.position.x, 0, 0);
        this._floatingBallsContainers.Add(data.Data);
    }

    public int PlayerId()
    {
        return this._gameZone.GameState.PlayerNumber;
    }
}