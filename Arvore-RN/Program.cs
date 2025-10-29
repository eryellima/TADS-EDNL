using ArvoreRubroNegra;


class Program{
    public static void Main(string[] args){
        ArvoreRN arvore = new ArvoreRN(50);
        arvore.PrintTree();
        Console.WriteLine("--------------------------------");

        arvore.Insert(25);
        arvore.Insert(75);
        arvore.PrintTree();
        Console.WriteLine("--------------------------------");

        arvore.Insert(10);
        arvore.Insert(30);
        arvore.PrintTree();
        Console.WriteLine("--------------------------------");

        arvore.Insert(60);
        arvore.Insert(80);
        arvore.PrintTree();
        Console.WriteLine("--------------------------------");

        
        Console.WriteLine("Testando Balanceamento da Árvore:");
        arvore.Insert(5);
        arvore.PrintTree();
        Console.WriteLine("--------------------------------");
        
        arvore.Insert(1);
        arvore.PrintTree();
    }
}