﻿using SkyCrab.Common_classes.Games.Tiles;
using System.Collections.Generic;
using System;

namespace SkyCrab.Common_classes.Games.Boards
{
    class StandardBoard : Board
    {

        private static readonly List<PositionOnBoard> squares;

        private static readonly SquareType[][] squareTypes = new SquareType[8][]
                {
					new SquareType[8] { SquareType.WORD3,   SquareType.NORMAL,  SquareType.NORMAL,  SquareType.LETTER2, SquareType.NORMAL,  SquareType.NORMAL,  SquareType.NORMAL,  SquareType.WORD3 },
					new SquareType[8] { SquareType.NORMAL,  SquareType.WORD2,   SquareType.NORMAL,  SquareType.NORMAL,  SquareType.NORMAL,  SquareType.LETTER3, SquareType.NORMAL,  SquareType.NORMAL },
					new SquareType[8] { SquareType.NORMAL,  SquareType.NORMAL,  SquareType.WORD2,   SquareType.NORMAL,  SquareType.NORMAL,  SquareType.NORMAL,  SquareType.LETTER2, SquareType.NORMAL },
					new SquareType[8] { SquareType.LETTER2, SquareType.NORMAL,  SquareType.NORMAL,  SquareType.WORD2,   SquareType.NORMAL,  SquareType.NORMAL,  SquareType.NORMAL,  SquareType.LETTER2 },
					new SquareType[8] { SquareType.NORMAL,  SquareType.NORMAL,  SquareType.NORMAL,  SquareType.NORMAL,  SquareType.WORD2,   SquareType.NORMAL,  SquareType.NORMAL,  SquareType.NORMAL },
					new SquareType[8] { SquareType.NORMAL,  SquareType.LETTER3, SquareType.NORMAL,  SquareType.NORMAL,  SquareType.NORMAL,  SquareType.LETTER3, SquareType.NORMAL,  SquareType.NORMAL },
					new SquareType[8] { SquareType.NORMAL,  SquareType.NORMAL,  SquareType.LETTER2, SquareType.NORMAL,  SquareType.NORMAL,  SquareType.NORMAL,  SquareType.LETTER2, SquareType.NORMAL },
					new SquareType[8] { SquareType.WORD3,   SquareType.NORMAL,  SquareType.NORMAL,  SquareType.LETTER2, SquareType.NORMAL,  SquareType.NORMAL,  SquareType.NORMAL,  SquareType.START }
				};

        private Tile[][] tiles;
        private uint count = 0;


        static StandardBoard()
        {
            squares = new List<PositionOnBoard>(15 * 15);
            for (int i = 0; i != 15; ++i)
                for (int j = 0; j != 15; ++j)
                    squares.Add(new PositionOnBoard(i, j));
        }


        public static bool Rectangle_
        {
            get { return true; }
        }
        public override bool Rectangle
        {
            get { return Rectangle_; }
        }

        public static PositionOnBoard LeftTop_
        {
            get { return new PositionOnBoard(0, 0); }
        }
        public override PositionOnBoard LeftTop
        {
            get { return LeftTop_; }
        }

        public static PositionOnBoard RightBottom_
        {
            get { return new PositionOnBoard(14, 14); }
        }
        public override PositionOnBoard RightBottom
        {
            get { return RightBottom_; }
        }

        public static IList<PositionOnBoard> Squares_
        {
            get { return squares; }
        }
        public override IList<PositionOnBoard> Squares
        {
            get { return Squares_; }
        }

        public static PositionOnBoard StartSquare_
        {
            get { return new PositionOnBoard(7, 7); }
        }
        public override PositionOnBoard StartSquare
        {
            get { return StartSquare_; }
        }

        public override uint Count
        {
            get { return count; }
        }

        public override bool IsEmpty
        {
            get { return count == 0; }
        }


        public StandardBoard()
        {
            tiles = new Tile[15][];
            for (uint i = 0; i != 15; ++i)
                tiles[i] = new Tile[15];
        }

        private StandardBoard(StandardBoard board)
        {
            tiles = new Tile[15][];
            for (uint i = 0; i != 15; ++i)
            {
                tiles[i] = new Tile[15];
                for (uint j = 0; j != 15; ++j)
                    tiles[i][j] = board.tiles[i][j];
            }
            count = board.count;
        }

        public static bool IsSquare_(PositionOnBoard position)
        {
            return position.x >= 0 && position.x < 15 &&
                    position.y >= 0 && position.y < 15;
        }
        public override bool IsSquare(PositionOnBoard position)
        {
            return IsSquare_(position);
        }

        public static SquareType GetSquareType_(PositionOnBoard position)
        {
            if (!IsSquare_(position))
                throw new NoSuchSquareOnBoardException();
            int x = (position.x <= 7) ? position.x : (14 - position.x);
            int y = (position.y <= 7) ? position.y : (14 - position.y);
            SquareType squareType = squareTypes[x][y];
            return squareType;
        }
        public override SquareType GetSquareType(PositionOnBoard position)
        {
            return GetSquareType_(position);
        }

        public override void PutTile(Tile tile, PositionOnBoard position)
        {
            if (GetTile(position) != null)
                throw new SquareOnBoardIsOccupiedException();
            tiles[position.x][position.y] = tile;
            ++count;
        }

        public override Tile GetTile(PositionOnBoard position)
        {
            if (!IsSquare(position))
                throw new NoSuchSquareOnBoardException();
            Tile tile = tiles[position.x][position.y];
            return tile;
        }

        public static string getSquareID_(PositionOnBoard position, bool horizontal)
        {
            return getSquareStandardID(position, horizontal);
        }
        public override string getSquareID(PositionOnBoard position, bool horizontal)
        {
            return getSquareID_(position, horizontal);
        }

        public override object Clone()
        {
            return new StandardBoard(this);
        }

    }
}
