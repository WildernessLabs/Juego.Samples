using Meadow;
using Meadow.Devices;
using Meadow.Foundation.Graphics;
using Meadow.Peripherals.Sensors.Hid;
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

            juego.MotionSensor.StartUpdating(TimeSpan.FromMilliseconds(200));

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
                var direction = GetPositionFromAccelerometer();

                for (int i = 0; i < board1.Length; i++)
                {
                    if (b1[i] == 1)
                    {
                        index = GetNewIndexForSand(b1, i, direction);
                        if (index != i)
                        {
                            b1[i] = 0;
                            b2[i] = 0;
                        }
                        b2[index] = 1;
                    }
                }

                CheckButtonsAndAddSand();

                useBoard1 = !useBoard1;

                graphics.Clear();

                DrawBoard();

                graphics.Show();

                Thread.Sleep(0);
            }
        }

        void CheckButtonsAndAddSand()
        {
            if (juego.Left_UpButton.State == true)
                AddSand(NUM_COLUMNS / 2, 0);
            if (juego.Left_DownButton.State == true)
                AddSand(NUM_COLUMNS / 2, NUM_ROWS - 1);
            if (juego.Left_LeftButton.State == true)
                AddSand(0, NUM_ROWS / 2);
            if (juego.Left_RightButton.State == true)
                AddSand(NUM_COLUMNS - 1, NUM_ROWS / 2);
        }

        DigitalJoystickPosition GetPositionFromAccelerometer()
        {
            if (juego.MotionSensor.Acceleration3D == null)
            {
                Console.WriteLine("No accelerometer data");
                return DigitalJoystickPosition.Center;
            }

            var accel = juego.MotionSensor.Acceleration3D.Value;

            if (accel.X.Gravity < -0.5)
            {
                if (accel.Y.Gravity > 0.5)
                    return DigitalJoystickPosition.UpLeft;
                else if (accel.Y.Gravity < -0.5)
                    return DigitalJoystickPosition.DownLeft;
                else
                    return DigitalJoystickPosition.Left;
            }
            else if (accel.X.Gravity > 0.5)
            {
                if (accel.Y.Gravity > 0.5)
                    return DigitalJoystickPosition.UpRight;
                else if (accel.Y.Gravity < -0.5)
                    return DigitalJoystickPosition.DownRight;
                else
                    return DigitalJoystickPosition.Right;
            }
            else if (accel.Y.Gravity > 0.5)
            {
                return DigitalJoystickPosition.Up;
            }
            else if (accel.Y.Gravity < -0.5)
            {
                return DigitalJoystickPosition.Down;
            }
            else
            {
                return DigitalJoystickPosition.Center;
            }
        }

        int GetNewIndexForDirection(byte[] board, int index,
            DigitalJoystickPosition direction,
            DigitalJoystickPosition directionAlt1,
            DigitalJoystickPosition directionAlt2)
        {
            if (CanMoveSand(board, index, direction))
            {
                return GetRawIndexForDirection(index, direction);
            }
            else
            {
                if (rand.Next(2) == 0)
                {
                    if (CanMoveSand(board, index, directionAlt1))
                        return GetRawIndexForDirection(index, directionAlt1);
                    else if (CanMoveSand(board, index, directionAlt2))
                        return GetRawIndexForDirection(index, directionAlt2);
                }
                else
                {
                    if (CanMoveSand(board, index, directionAlt2))
                        return GetRawIndexForDirection(index, directionAlt2);
                    else if (CanMoveSand(board, index, directionAlt1))
                        return GetRawIndexForDirection(index, directionAlt1);
                }
            }
            return index;
        }

        int GetNewIndexForSand(byte[] board, int index, DigitalJoystickPosition gravityDirection)
        {
            return gravityDirection switch
            {
                DigitalJoystickPosition.Up => GetNewIndexForDirection(board, index, DigitalJoystickPosition.Up, DigitalJoystickPosition.UpLeft, DigitalJoystickPosition.UpRight),
                DigitalJoystickPosition.UpLeft => GetNewIndexForDirection(board, index, DigitalJoystickPosition.UpLeft, DigitalJoystickPosition.Up, DigitalJoystickPosition.Left),
                DigitalJoystickPosition.UpRight => GetNewIndexForDirection(board, index, DigitalJoystickPosition.UpRight, DigitalJoystickPosition.Up, DigitalJoystickPosition.Right),
                DigitalJoystickPosition.Down => GetNewIndexForDirection(board, index, DigitalJoystickPosition.Down, DigitalJoystickPosition.DownLeft, DigitalJoystickPosition.DownRight),
                DigitalJoystickPosition.DownLeft => GetNewIndexForDirection(board, index, DigitalJoystickPosition.DownLeft, DigitalJoystickPosition.Down, DigitalJoystickPosition.Left),
                DigitalJoystickPosition.DownRight => GetNewIndexForDirection(board, index, DigitalJoystickPosition.DownRight, DigitalJoystickPosition.Down, DigitalJoystickPosition.Right),
                DigitalJoystickPosition.Left => GetNewIndexForDirection(board, index, DigitalJoystickPosition.Left, DigitalJoystickPosition.UpLeft, DigitalJoystickPosition.DownLeft),
                DigitalJoystickPosition.Right => GetNewIndexForDirection(board, index, DigitalJoystickPosition.Right, DigitalJoystickPosition.UpRight, DigitalJoystickPosition.DownRight),
                _ => index,
            };
        }
        int GetRawIndexForDirection(int index, DigitalJoystickPosition position)
        {
            return position switch
            {
                DigitalJoystickPosition.Up => index - NUM_COLUMNS,
                DigitalJoystickPosition.UpRight => index - NUM_COLUMNS + 1,
                DigitalJoystickPosition.Right => index + 1,
                DigitalJoystickPosition.DownRight => index + NUM_COLUMNS + 1,
                DigitalJoystickPosition.Down => index + NUM_COLUMNS,
                DigitalJoystickPosition.DownLeft => index + NUM_COLUMNS - 1,
                DigitalJoystickPosition.Left => index - 1,
                DigitalJoystickPosition.UpLeft => index - NUM_COLUMNS - 1,
                _ => index,
            };
        }

        bool IsOnEdgeForDirection(int index, DigitalJoystickPosition position)
        {
            return position switch
            {
                DigitalJoystickPosition.Up => IsTopEdge(index),
                DigitalJoystickPosition.UpRight => IsTopEdge(index) || IsRightEdge(index),
                DigitalJoystickPosition.Right => IsRightEdge(index),
                DigitalJoystickPosition.DownRight => IsBottomEdge(index) || IsRightEdge(index),
                DigitalJoystickPosition.Down => IsBottomEdge(index),
                DigitalJoystickPosition.DownLeft => IsBottomEdge(index) || IsLeftEdge(index),
                DigitalJoystickPosition.Left => IsLeftEdge(index),
                DigitalJoystickPosition.UpLeft => IsTopEdge(index) || IsLeftEdge(index),
                _ => false,
            };
        }

        bool CanMoveSand(byte[] board, int index, DigitalJoystickPosition position)
        {
            if (IsOnEdgeForDirection(index, position)) return false;
            if (board[GetRawIndexForDirection(index, position)] == 0) return true;
            return false;
        }

        bool IsLeftEdge(int index) => index % NUM_COLUMNS == 0;
        bool IsRightEdge(int index) => index % NUM_COLUMNS == NUM_COLUMNS - 1;
        bool IsTopEdge(int index) => index / NUM_COLUMNS == 0;
        bool IsBottomEdge(int index) => index / NUM_COLUMNS == NUM_ROWS - 1;

        public void AddSand(int x, int y)
        {
            if (useBoard1) board1[x + y * NUM_COLUMNS] = 1;
            else board2[x + y * NUM_COLUMNS] = 1;
        }

        void ClearBoards()
        {
            Array.Clear(array: board1, index: 0, length: board1.Length);
        }

        void DrawBoard()
        {
            var board = useBoard1 ? board1 : board2;

            for (int i = 0; i < board1.Length; i++)
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