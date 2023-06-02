using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Snack_game
{
    class Program
    {
       

         
            #region parameter
            public Random rand = new Random();
            public ConsoleKeyInfo keypress = new ConsoleKeyInfo();
            int score, headX, headY, X_vi_tri_qua, Y_vi_tri_qua, Chieu_dai_con_ran;
            int Gia_tri_qua;
            int[] TailX = new int[100];
            int[] TailY = new int[100];
            const int height = 20;
            const int width = 60;
            const int panel = 10;
            bool gameOver, reset, isprinted, horizontal, vertical;
            string dir, pre_dir;
            #endregion
            //Hiển thị màn hình khi bắt đầu
            void ShowBanner()
            {
                Console.SetWindowSize(width, height + panel);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.CursorVisible = false; //tắt con trỏ nháy nháy
                Console.WriteLine("===SNAKE GAME===");
                Console.WriteLine("Press any key to play");
                Console.WriteLine("Tips: - Press P key to PAUSE game");
                Console.WriteLine("      - Press R key to RESET game");
                Console.WriteLine("      - Press Q key to QUIT game");

                keypress = Console.ReadKey(true);
                if (keypress.Key == ConsoleKey.Q)
                    Environment.Exit(0);
            }

            //Cài đặt thông số ban đầu
            void Setup()
            {
                dir = "RIGHT"; pre_dir = "";    //bước đi đầu tiên qua phải 
                score = 0; Chieu_dai_con_ran = 0;
                gameOver = reset = isprinted = false;
                headX = 20; //ko vuot qua width (vi tri bat dau Ran)
                headY = 10; //ko vuot qua height(vi tri bat dau Ran)
                randonQua();
            }

            //Sinh ngẫu nhiên vị trí quả
            void randonQua()
            {
                Gia_tri_qua = rand.Next(1, 5);
                X_vi_tri_qua = rand.Next(1, width - 1);
                Y_vi_tri_qua = rand.Next(1, height - 1);
            }

            //Cập nhật màn hình
            void Update()
            {
                while (!gameOver)
                {
                    CheckInput();
                    Logic();
                    Render();
                    if (reset) break;
                    //DUng man hinh 1s
                    if (score < 3)
                    {
                        Thread.Sleep(30);
                    }
                    else if (score < 5)
                    {
                        Thread.Sleep(20);
                    }
                    else if (score < 10)
                    {
                        Thread.Sleep(15);
                    }

                }
                if (gameOver) Lose();
            }

            //Điều khiển phím
            void CheckInput()
            {
                while (Console.KeyAvailable)
                {
                    //Cho bam phim bat ky
                    keypress = Console.ReadKey(true);
                    //dir -> pre_dir
                    pre_dir = dir;  //luu lai huong di truoc do
                    switch (keypress.Key)
                    {
                        case ConsoleKey.Q: Environment.Exit(0); break;
                        case ConsoleKey.P: dir = "STOP"; break;
                        case ConsoleKey.LeftArrow: dir = "LEFT"; break;
                        case ConsoleKey.RightArrow: dir = "RIGHT"; break;
                        case ConsoleKey.UpArrow: dir = "UP"; break;
                        case ConsoleKey.DownArrow: dir = "DOWN"; break;
                    }
                }

            }

            //Kiểm tra điều hướng
            void Logic()
            {
                int preX = TailX[0], preY = TailY[0]; // (x,y)
                int tempX, tempY;
                //0 1 2 3 4 => Con rắn ăn thêm quà            //x 0 1 2 3 4
                if (dir != "STOP")
                {
                    TailX[0] = headX; TailY[0] = headY;

                    for (int i = 1; i < Chieu_dai_con_ran; i++)
                    {
                        tempX = TailX[i]; tempY = TailY[i];
                        TailX[i] = preX; TailY[i] = preY;
                        preX = tempX; preY = tempY;
                    }
                }
                switch (dir)
                {
                    case "RIGHT": headX++; break;
                    case "LEFT": headX--; break;
                    case "DOWN": headY++; break;
                    case "UP": headY--; break;
                    case "STOP":
                        {
                            while (true)
                            {
                                Console.Clear();
                                Console.WriteLine("Game pause!");
                                Console.WriteLine("- Press P key to PAUSE game");
                                Console.WriteLine("- Press R key to RESET game");
                                Console.WriteLine("- Press Q key to QUIT game");

                                keypress = Console.ReadKey(true);
                                if (keypress.Key == ConsoleKey.Q)
                                    Environment.Exit(0);
                                if (keypress.Key == ConsoleKey.R)
                                {
                                    reset = true; break; //choi lai game
                                }
                                if (keypress.Key == ConsoleKey.P)
                                    break;  //choi tiep game
                            }
                        }
                        dir = pre_dir; break;
                }
                #region
                //kiem tra va cham bien (le trai, phai)
                // if (headX <= 0 || headX >= width - 1 ||
                //   headY <= 0 || headY >= height - 1) gameOver = true;
                //else gameOver = false;
                #endregion
                //kiem tra an qua
                if (headX == X_vi_tri_qua && headY == Y_vi_tri_qua)
                {

                    score += Gia_tri_qua; Chieu_dai_con_ran += Gia_tri_qua;    //tinh diem khi an qua
                    randonQua();            //random diem qua moi
                }

                if (((dir == "LEFT" && pre_dir != "UP") && (dir == "LEFT" && pre_dir != "DOWN")) ||
                    ((dir == "RIGHT" && pre_dir != "UP") && (dir == "RIGHT" && pre_dir != "DOWN")))
                    horizontal = true;
                else horizontal = false;

                if (((dir == "UP" && pre_dir != "LEFT") && (dir == "UP" && pre_dir != "RIGHT")) ||
                    ((dir == "DOWN" && pre_dir != "LEFT") && (dir == "DOWN" && pre_dir != "RIGHT")))
                    vertical = true;
                else vertical = false;

                //kiem tra cai dau va cham than con ran
                for (int i = 1; i < Chieu_dai_con_ran; i++)
                {
                    if (headX == TailX[i] && headY == TailY[i])
                    {
                        if (horizontal || vertical) gameOver = false; //quay dau 180 độ [bẫy lỗi quay đầu]
                        else gameOver = true;
                    }
                    if (X_vi_tri_qua == TailX[i] && Y_vi_tri_qua == TailY[i]) //quà trùng thân con rắn cho random lại
                        randonQua();
                }
            }

            //Hiển thị màn hình
            void Render()
            {
                Console.SetCursorPosition(0, 0);
                for (int i = 0; i < height; i++)
                {
                    for (int j = 0; j < width; j++)
                    {
                        if (i == 0 || i == height - 1)   //viền khung trên và dưới
                            Console.Write("-");
                        else if (j == 0 || j == width - 1) //viền khung trái và phải
                            Console.Write("|");
                        else if (j == X_vi_tri_qua && i == Y_vi_tri_qua) //hộp quả
                        {
                            switch (Gia_tri_qua)
                            {
                                case (1): Console.Write("1"); break;
                                case (2): Console.Write("2"); break;
                                case (3): Console.Write("3"); break;
                                case (4): Console.Write("4"); break;
                                case (5): Console.Write("5"); break;
                            }



                        }

                        else if (j == headX && i == headY) //đầu con rắn
                            Console.Write("o");
                        else if (headX <= 0)
                        {
                            if (j == width - 1 && i == headY)
                            {
                                Console.Write("o");
                                //headX--;
                                Console.Write("o");
                            }
                        }
                        //kiem tra va cham bien (le trai, phai)
                       else if( headX <= 0 || headX >= width - 1 ||
                         headY <= 0 || headY >= height - 1) gameOver = true;
                        else
                        {
                            isprinted = false;
                            for (int k = 0; k < Chieu_dai_con_ran; k++)
                            {
                                if (TailX[k] == j && TailY[k] == i)
                                {
                                    Console.ForegroundColor = ConsoleColor.Red;
                                    Console.Write("*");  //thân con rắn
                                    Console.ForegroundColor = ConsoleColor.Blue;

                                    isprinted = true;
                                }
                            }
                            if (!isprinted) Console.Write(" "); //vị trí còn lại
                        }
                    }
                    Console.WriteLine();
                }
                //Hiển thị panel thông tin điểm phía dưới khung viền
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Your score: " + score);
            }

            //Xử lý khi thua
            void Lose()
            {
                Console.WriteLine("YOU DIED");
                Console.WriteLine("      - Press R key to RESET game");
                Console.WriteLine("      - Press Q key to QUIT game");

                while (true)
                {
                    keypress = Console.ReadKey(true);
                    if (keypress.Key == ConsoleKey.Q)
                        Environment.Exit(0);
                    if (keypress.Key == ConsoleKey.R)
                        reset = true; break;
                }
            }

            static void Main(string[] args)
            {
            Program snakegame = new Program(); //game ko xac dinh diem dung
                snakegame.ShowBanner();
                while (true)
                {
                    snakegame.Setup();
                    snakegame.Update();
                    Console.Clear(); //Xoa man hinh hien thi
                }
            }
        }



    }
