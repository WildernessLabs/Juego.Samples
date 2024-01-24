using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using System;
using System.Threading;
using System.Threading.Tasks;
using WildernessLabs.Hardware.Juego;

namespace FallingSand
{
    // Change F7FeatherV2 to F7FeatherV1 for V1.x boards
    public class MeadowApp : App<F7CoreComputeV2>
    {
        IJuegoHardware juego;
        MicroGraphics graphics;

        readonly Random rand = new Random();

        static readonly int NUM_COLUMNS = 80; //160;
        static readonly int NUM_ROWS = 60; //120;

        readonly int SIZE = 4; //2;

        readonly byte[] board1 = new byte[NUM_COLUMNS * NUM_ROWS];
        readonly byte[] board2 = new byte[NUM_COLUMNS * NUM_ROWS];

        bool useBoard1 = true;

        public override Task Initialize()
        {
            Console.WriteLine("Initialize...");

            juego = Juego.Create();

            graphics = new MicroGraphics(juego.Display);

            ClearBoards();

            Console.WriteLine("Initialize complete");

            return Task.CompletedTask;
        }

        public override Task Run()
        {
            graphics.Clear(true);
            byte[] b1, b2;
            int index;

            while (true)
            {
                b1 = useBoard1 ? board1 : board2;
                b2 = useBoard1 ? board2 : board1;

                for (int i = 0; i < board1.Length; i++)
                {
                    if (b1[i] == 1)
                    {
                        index = UpdateSand(b1, i);
                        b1[i] = 0;
                        b2[i] = 0;
                        b2[index] = 1;
                    }
                }

                if (juego.Left_DownButton.State == true)
                {
                    AddSand(NUM_COLUMNS / 2, 0);
                }

                useBoard1 = !useBoard1;

                graphics.Clear();

                DrawBoard();

                graphics.Show();

                Thread.Sleep(0);


            }
        }

        int UpdateSand(byte[] board, int index)
        {
            // If we're at the bottom of the board, return the index
            if (index / NUM_COLUMNS >= NUM_ROWS - 1)
            {
                return index;
            }
            //if we're at left edge of board, return index
            else if (index % NUM_COLUMNS == 0)
            {
                return index;
            }
            //if we're at right edge of board, return index
            else if (index % NUM_COLUMNS == NUM_COLUMNS - 1)
            {
                return index;
            }
            // If there's nothing below us, move down
            else if (board[index + NUM_COLUMNS] == 0)
            {
                return index + NUM_COLUMNS;
            }
            // If there's nothing below and to the left, move down and left
            else if (board[index + NUM_COLUMNS - 1] == 0)
            {
                return index + NUM_COLUMNS - 1;
            }
            // If there's nothing below and to the right, move down and right
            else if (board[index + NUM_COLUMNS + 1] == 0)
            {
                return index + NUM_COLUMNS + 1;
            }
            // Otherwise, stay where we are
            else
            {
                return index;
            }
        }

        public void AddSand(int x, int y)
        {
            if (useBoard1)
            {
                board1[x + y * NUM_COLUMNS] = 1;
            }
            else
            {
                board2[x + y * NUM_COLUMNS] = 1;
            }
        }

        void ClearBoards()
        {
            Array.Clear(array: board1, index: 0, length: board1.Length);
            Array.Clear(array: board2, index: 0, length: board2.Length);
        }

        void DrawBoard()
        {
            var board = useBoard1 ? board1 : board2;

            for (int i = 0; i < board.Length; i++)
            {
                if (board[i] == 1)
                {
                    int x = i % NUM_COLUMNS;
                    int y = i / NUM_COLUMNS;

                    graphics.DrawRectangle(x * SIZE, y * SIZE, SIZE, SIZE, Color.SandyBrown, true);
                }
            }
        }
    }
}