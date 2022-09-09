using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallFactory
{
    private static BallFactory INSTANCE;

    private static int[][] BEARINGS_LEVEL =
    {
        new[] {3},
        new[] {4, 5},
        new[] {6, 7},
        new[] {8, 9},
        new[] {10, 11},
        new[] {12, 13},
        new[] {14, 15},
        new[] {16, 17},
        new[] {17, 19}
    };

    private static int[] COUNTER_PU = {GameState.BALL_BY_LEVEL + 1, 8, 8, 7, 7, 7, 6, 6, 5};

    //Value for Difficulty Curves
    private static int countAtMinLevel = COUNTER_PU[COUNTER_PU.Length - 1];
    private static int minCount = 2;
    private static int minLevel = 20;
    private static int levelAtMinCount = 25;
    private static float importanceMultiplicator = 2;
    private static float inclination = 0.7f;

    public enum PuType
    {
        JokerType,
        BombType,
        ZapHorizontalType,
        ZapDiagonalType,
        CutterType,
        PlasmaNoTriangleType,
        PlasmaEmptyTriangleType,
        BrickSquareType,
        StingType,
        TransformBrickType,
        TransformDestroyType,
        TransformJokerType,
        CopyLineType,
        CopySquareType,
        CopyPredictionType,
        TransformBombType,
        BrickTowerType,
        PlasmaTriangleType
    }

    private static PuType[] PUTypes =
    {
        PuType.JokerType,
        PuType.BombType,
        PuType.ZapHorizontalType,
        PuType.ZapDiagonalType,
        PuType.CutterType,
        PuType.PlasmaNoTriangleType,
        PuType.PlasmaEmptyTriangleType,
        PuType.BrickSquareType,
        PuType.StingType,
        PuType.TransformBrickType,
        PuType.TransformDestroyType,
        PuType.TransformJokerType,
        PuType.CopyLineType,
        PuType.CopySquareType,
        PuType.CopyPredictionType,
        PuType.TransformBombType,
        PuType.BrickTowerType,
        PuType.PlasmaTriangleType
    };

    private static Dictionary<PuType, float[]> PROBABILITY_PU = new Dictionary<PuType, float[]>
    {
        {PuType.JokerType, new[] {0.5f, 0.5f, 0.25f, 0.2f, 0.1f, 0.05f, 0, 0, 0, 0}},
        {PuType.BombType, new[] {0.5f, 0.5f, 0.25f, 0.2f, 0.1f, 0.08f, 0.07f, 0.07f,0.07f, 0.06f}},
        {PuType.ZapHorizontalType, new[] {0, 0, 0.25f, 0.20f, 0.10f, 0.09f, 0.07f, 0.07f, 0.07f, 0.06f}},
        {PuType.ZapDiagonalType, new[] {0, 0, 0.25f, 0.20f, 0.10f, 0.09f, 0.07f, 0.07f, 0.07f, 0.06f}},
        {PuType.CutterType, new[] {0, 0, 0, 0.20f, 0.10f, 0.08f, 0.07f, 0.07f, 0.07f, 0.06f}},
        {PuType.PlasmaNoTriangleType, new[] {0, 0, 0, 0, 0.13f, 0.08f, 0.05f, 0.03f, 0, 0}},
        {PuType.PlasmaEmptyTriangleType, new[] {0, 0, 0, 0, 0.13f, 0.11f, 0.08f, 0.05f, 0.03f, 0}},
        {PuType.BrickSquareType, new[] {0, 0, 0, 0, 0.13f, 0.10f, 0.07f, 0.07f, 0.06f, 0.04f}},
        {PuType.StingType, new[] {0, 0, 0, 0, 0.11f, 0.10f, 0.08f, 0.06f, 0.06f, 0.06f}},
        {PuType.TransformBrickType, new[] {0, 0, 0, 0, 0, 0.11f, 0.08f, 0.08f, 0.08f, 0.06f}},
        {PuType.TransformDestroyType, new[] {0, 0, 0, 0, 0, 0.11f, 0.09f, 0.09f, 0.08f, 0.06f}},
        {PuType.TransformJokerType, new[] {0, 0, 0, 0, 0, 0, 0.09f, 0.09f, 0.08f, 0.05f}},
        {PuType.CopyLineType, new[] {0, 0, 0, 0, 0, 0, 0.09f, 0.08f, 0.07f, 0.06f}},
        {PuType.CopySquareType, new[] {0, 0, 0, 0, 0, 0, 0.09f, 0.08f, 0.07f, 0.06f}},
        {PuType.CopyPredictionType, new[] {0, 0, 0, 0, 0, 0, 0, 0.09f, 0.09f, 0.08f}},
        {PuType.TransformBombType, new[] {0, 0, 0, 0, 0, 0, 0, 0, 0.1f, 0.1f}},
        {PuType.BrickTowerType, new[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0.09f}},
        {PuType.PlasmaTriangleType, new[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0.1f}},
    };


    private BallFactory()
    {
    }

    public static BallFactory getInstance()
    {
        if (INSTANCE is null)
        {
            INSTANCE = new BallFactory();
        }

        return INSTANCE;
    }


    private int getBearingIndex(GameState gs)
    {
        for (int i = 0; i < BEARINGS_LEVEL.Length; i++)
        {
            for (int j = 0; j < BEARINGS_LEVEL[i].Length; j++)
            {
                if (BEARINGS_LEVEL[i][j] == gs.Level) return i;
            }
        }

        return BEARINGS_LEVEL.Length;
    }


    public void RefreshPu(GameState gs)
    {
        int bearingIndex = getBearingIndex(gs);
        int counterPu;
        if (bearingIndex == BEARINGS_LEVEL.Length)
        {
            float multCompute = (gs.Multiplicator - GameState.MULTIPLICATOR_MIN) /
                (GameState.MULTIPLICATOR_MAX - GameState.MULTIPLICATOR_MIN) - 1;
            float val1 = minCount - multCompute * importanceMultiplicator;
            float val2 = gs.Level <= levelAtMinCount
                ? (countAtMinLevel - val1) / (minLevel - levelAtMinCount)
                : inclination;
            float val3 = gs.Level - levelAtMinCount;
            counterPu = (int) Math.Round(val2 * val3 + val1);
        }
        else
        {
            counterPu = COUNTER_PU[bearingIndex] + GameState.MULTIPLICATOR_MAX - gs.Multiplicator;
        }

        gs.NextPu = GeneratePu(gs);
        gs.CountPowerUp =
            GameState.BALL_BY_LEVEL - gs.NbBallDrop % GameState.BALL_BY_LEVEL == counterPu % GameState.BALL_BY_LEVEL
                ? counterPu + 1
                : counterPu;
    }

    private SpecialBall GeneratePu(GameState gs)
    {
        float randProbability = Random.Range(0, Int32.MaxValue) / (float) Int32.MaxValue;
        float countProbability = 0;

        Array puENum = Enum.GetValues(typeof(PuType));
        PuType lastPuType = PUTypes[0];
        foreach (int i in puENum)
        {
            int bearingIndex = getBearingIndex(gs);
            float curProbability = PROBABILITY_PU[PUTypes[i]][bearingIndex];
            if (curProbability != 0) lastPuType = PUTypes[i];

            countProbability += curProbability;
            if (randProbability < countProbability)
            {
                return generatePUByID(PUTypes[i]);
            }
        }

        return generatePUByID(lastPuType);
    }

    private SpecialBall generatePUByID(PuType puType)
    {
        switch (puType)
        {
            case PuType.BombType:
                return new BombBall();
            case PuType.JokerType:
                return new JokerBall();
            case PuType.CutterType:
                return new CutterBall();
            case PuType.ZapHorizontalType:
                return new ZapHorizontalBall();
            case PuType.ZapDiagonalType:
                return new ZapDiagonalBall();
            case PuType.PlasmaNoTriangleType:
                return new PlasmaNoTriangleBall();
            case PuType.BrickSquareType:
                return new BrickSquare();
            case PuType.CopyLineType:
                return new CopyLineBall();
            case PuType.CopySquareType:
                return new CopySquareBall();
            case PuType.PlasmaEmptyTriangleType:
                return new PlasmaEmptyTriangleBall();
            case PuType.TransformJokerType:
                return new TransformJokerBall();
            case PuType.TransformBrickType:
                return new TransformBrickBall();
            case PuType.CopyPredictionType:
                return new CopyPredictionBall();
            case PuType.TransformDestroyType:
                return new TransformDestroyBall();
            case PuType.TransformBombType:
                return new TransformBombBall();
            case PuType.PlasmaTriangleType:
                return new PlasmaFullTriangleBall();
            case PuType.StingType:
                return new StingBall();
            case PuType.BrickTowerType:
                return new BrickTower();
            default:
                return null;
        }
    }

    public Ball GenerateBall(GameState gs)
    {
        if (gs.NbBallDrop != 0 && gs.NbBallDrop % GameState.BALL_BY_LEVEL == 0)
        {
            return new StarBall();
        }

        if (gs.CountPowerUp == 0)
        {
            SpecialBall pu = gs.NextPu;
            RefreshPu(gs);
            return pu;
        }

        return new NormalBall(Random.Range(1, gs.Level + 1), Random.Range(0, gs.Level));
    }

    public void RefreshLevelBall(GameState gs)
    {
        gs.LevelBall = new NormalBall(gs.Level, gs.Level - 1);
    }

    public Ball GetTransformedFloatingBall(Ball b)
    {
        Ball res = null;
        if (b.IsNormalBall || b.GetType() == typeof(BombBall))
        {
            res = new JokerBall();
        }
        else
        {
            res = new BombBall();
        }

        return res;
    }
}