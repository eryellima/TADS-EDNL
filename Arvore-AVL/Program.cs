class Program{
    public static void Main(string[] args){
        ArvoreAVL arvore = new ArvoreAVL(50);
        arvore.PrintTree();
        Console.WriteLine("--------------------------------");

        arvore.InsertAVL(25);
        arvore.InsertAVL(75);
        arvore.PrintTree();
        Console.WriteLine("--------------------------------");

        arvore.InsertAVL(10);
        arvore.InsertAVL(30);
        arvore.PrintTree();
        Console.WriteLine("--------------------------------");

        arvore.InsertAVL(60);
        arvore.InsertAVL(80);
        arvore.PrintTree();
        Console.WriteLine("--------------------------------");

        
        Console.WriteLine("Testando Balanceamento da Árvore:");
        arvore.InsertAVL(5);
        arvore.PrintTree();
        Console.WriteLine("--------------------------------");
        
        arvore.InsertAVL(1);
        arvore.PrintTree();
        Console.WriteLine("--------------------------------");

        Console.WriteLine("Removendo Elementos 25:");
        arvore.RemoveAVL(25);
        arvore.PrintTree();
        Console.WriteLine("--------------------------------");

        Console.WriteLine("Removendo Elementos 30:");
        arvore.RemoveAVL(30);
        arvore.PrintTree();
        Console.WriteLine("--------------------------------");

    }
}