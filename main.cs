using System;

class Cell
{
    public int num = 0; // Número de minas ao redor
    public bool isMine = false;
    public bool isRevealed = false;
    public bool isFlagged = false;
}

class Grid
{
    public bool isGameOver = false; // Indica se o jogo foi perdido
    public bool isGameWon = false; // Indica se o jogo foi ganho
    private int nBombas = 10;
    private int tamanho = 10; // Tamanho da grid NxN
    public Cell[,] grid;
    private Random rand = new Random(); // Criação do Random fora do loop

    public Grid(int nBombas, int tamanho)
    {
        this.nBombas = nBombas;
        this.tamanho = tamanho;
        grid = new Cell[tamanho, tamanho];
        InicializarGrid();
    }

    private void InicializarGrid()
    {
        SetarGrid();
        SetarBombas();
    }

    private void SetarGrid()
    {
        for (int i = 0; i < tamanho; i++)
            for (int j = 0; j < tamanho; j++)
                grid[i, j] = new Cell(); // Inicializa cada célula como um novo objeto
    }

    private void SetarBombas()
    {
        for (int i = 0; i < nBombas; i++)
        {
            int x, y;
            do
            {
                x = rand.Next(0, tamanho);
                y = rand.Next(0, tamanho);
            } while (grid[y, x].isMine);

            grid[y, x].isMine = true;
            setarNumeros(y, x);
        }
    }

    private void setarNumeros(int y, int x)
    {
        for (int i = y - 1; i <= y + 1; i++)
        {
            for (int j = x - 1; j <= x + 1; j++)
            {
                if (i >= 0 && i < tamanho && j >= 0 && j < tamanho && !grid[i, j].isMine)
                {
                    grid[i, j].num++;
                }
            }
        }
    }

    public void RevelaCelula(int y, int x)
    {
        if (grid[y, x].isRevealed || grid[y, x].isFlagged) return;

        grid[y, x].isRevealed = true;

        if (grid[y, x].isMine)
        {
            isGameOver = true;
            ExibirMensagemDerrota();
            return;
        }

        if (grid[y, x].num == 0)
        {
            // Revela células vizinhas se não tiver minas ao redor
            for (int i = y - 1; i <= y + 1; i++)
            {
                for (int j = x - 1; j <= x + 1; j++)
                {
                    if (i >= 0 && i < tamanho && j >= 0 && j < tamanho && !grid[i, j].isRevealed && !grid[i, j].isFlagged && !grid[i, j].isMine)
                    {
                        RevelaCelula(i, j);
                    }
                }
            }
        }

        VerificarVitoria();
    }

    public void ToggleFlag(int y, int x)
    {
        if (grid[y, x].isRevealed) return;

        grid[y, x].isFlagged = !grid[y, x].isFlagged;
    }

    private void VerificarVitoria()
    {
        foreach (var celula in grid)
        {
            if (!celula.isMine && !celula.isRevealed)
                return; // Ainda há células seguras não reveladas
        }

        isGameWon = true;
        ExibirMensagemVitoria();
    }

    public void ExibirGrid()
    {
        Console.Clear();
        Console.WriteLine("Campo Minado:\n");

        for (int y = 0; y < tamanho; y++)
        {
            for (int x = 0; x < tamanho; x++)
            {
                Cell celula = grid[y, x];

                if (!celula.isRevealed)
                {
                    if (celula.isFlagged)
                        Console.Write("F "); // Célula marcada com bandeira
                    else
                        Console.Write("# "); // Célula escondida
                }
                else
                {
                    if (celula.isMine)
                        Console.Write("* "); // Mina revelada
                    else
                        Console.Write(celula.num + " "); // Número de minas ao redor
                }
            }
            Console.WriteLine();
        }
    }

    public void ProcessarEntrada(string input)
    {
        var comandos = input.Split(' ');
        int x = int.Parse(comandos[1]);
        int y = int.Parse(comandos[2]);

        if (comandos[0] == "0") // Revela célula
        {
            RevelaCelula(y, x);
        }
        else if (comandos[0] == "1") // Marca bandeira
        {
            ToggleFlag(y, x);
        }
    }

    private void ExibirMensagemVitoria()
    {
        Console.WriteLine("Parabéns! Você venceu o jogo!");
    }

    private void ExibirMensagemDerrota()
    {
        Console.WriteLine("Game Over! Você revelou uma mina.");
    }
}

class Program
{
    static void Main()
    {
        Grid grid = new Grid(10, 10); // 10 minas e uma grid 10x10
        string input;

        while (!grid.isGameOver && !grid.isGameWon)
        {
            grid.ExibirGrid();

            Console.WriteLine("Digite '0 X Y' para revelar uma célula ou '1 X Y' para colocar uma bandeira.");
            input = Console.ReadLine();

            grid.ProcessarEntrada(input);
        }
    }
}
