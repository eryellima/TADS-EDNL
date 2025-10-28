using System.Collections;
using System.Xml;

public class ArvoreAVL{
    private int tamanho;
    private Node? Raiz;
    private ArrayList elementos = new ArrayList();


    // Métodos de Árvore Binária de Busca
    public int Size(){
        return this.tamanho;
    }

    public Node? Root(){
        return this.Raiz;
    }

    public bool IsRoot(Node n){
        return n == this.Raiz;
    }

    public bool IsExternal(Node n){
        return n.FilhoEsquerdo == null && n.FilhoDireito == null;
    }

    public bool IsInternal(Node n){
        return !IsExternal(n);
    }

    public int Depth(Node n){
        if(IsRoot(n)){
            return 0;
        } else{
            return 1 + Depth(n.Pai);
        }
    }

    public int Height(Node n){
        if(n == null || IsExternal(n)){
            return 0;
        } else{
            return 1 + Math.Max(Height(n.FilhoEsquerdo), Height(n.FilhoDireito));
        }
    }


    public Node Search(int v){
        return Search(Raiz, v);
    }

    private Node Search(Node n, int v){
        if (IsExternal(n)){
            return n;
        }

        // Valor menor irá caminhar pela esquerda
        if(v < n.Valor){
            if(n.FilhoEsquerdo != null){
                return Search(n.FilhoEsquerdo, v);
            }
        }

        // Valor maior irá caminhar pela direita
        if(v > n.Valor){
            if(n.FilhoDireito != null){
                return Search(n.FilhoDireito, v);
            }
        }
        // Valor igual retorna o próprio nó
        return n;
    }

    
    private Node Insert(int v){
        if(Raiz == null){
            Raiz = new Node(null, v);
            tamanho++;
            return Raiz;
        }

        Node n = Search(v);
        
        if(n.Valor == v){
            throw new Exception("Valor já existe na árvore");
        }
        
        Node novoNo = new Node(n, v);
        
        if(v < n.Valor){
            n.FilhoEsquerdo = novoNo;
        }

        if (v > n.Valor){
            n.FilhoDireito = novoNo;
        }

        tamanho++;
        return novoNo;
    }


    private Node Remove(int v){
        Node n = RemoveAux(v);
        tamanho--;
        return n;
    }

    private Node RemoveAux(int v){
        Node n = Search(v);

        if(n.Valor != v){
            throw new Exception("Valor não encontrado na árvore");
        }

        int tipo = CaseType(n);

        switch(tipo){
            // Caso 0 == o nó é a raiz e não possui filhos
            case 0:
                RemoveRoot();
                break;

            // Caso 1 == o nó é uma folha
            case 1:
                RemoveLeaf(n);
                break;

            // Caso 2 == o nó possui um único filho
            case 2:
                RemoveOneChild(n);
                break;

            // Caso 3 == o nó possui dois filhos
            case 3:
                RemoveTwoChild(n);
                break;
        }
        return n;
    }


    private int CaseType(Node n){
        // Caso 0: o nó é a raiz e não possui filhos
        if(IsRoot(n) && IsExternal(n)){
            return 0;
        }

        // Caso 1: o nó é uma folha
        if(IsExternal(n)){
            return 1;
        }

        // Caso 2: o nó possui um único filho
        if(n.FilhoEsquerdo != null || n.FilhoDireito != null){
            return 2;
        }

        // Caso 3: o nó possui dois filhos
        if(n.FilhoEsquerdo != null && n.FilhoDireito != null){
            return 3;
        }

        return -1;
    }


    private void RemoveRoot(){
        Raiz = null;
    }


    private void RemoveLeaf(Node n){
        Node p = n.Pai;

        // Verifica de que lado n é filho
        if(n == p.FilhoEsquerdo){
            p.FilhoEsquerdo = null;
        } else{
            p.FilhoDireito = null;
        }
    }


    private void RemoveOneChild(Node n){
        Node p = n.Pai;
        Node f; // Filho de n

        // Verifica que lado está o filho de n
        if(n.FilhoEsquerdo != null){
            f = n.FilhoEsquerdo;
        } else{
            f = n.FilhoDireito;
        }

        // Verifica de que lado n é filho
        if(n == p.FilhoEsquerdo){
            p.FilhoEsquerdo = f;
            f.Pai = p;
        } else{
            p.FilhoDireito = f;
            f.Pai = p;
        }
    }


    private void RemoveTwoChild(Node n){
        Node s = FindSucessor(n.FilhoDireito); // Sucessor de n
    
        if(s != null){
            int temp = s.Valor;
            RemoveAux(temp);
            n.Valor = temp;
        }
    }


    public Node FindSucessor(Node n){
        if(n.FilhoEsquerdo != null){
            FindSucessor(n.FilhoEsquerdo);
        }
        return n;
    }


    // Métodos da Árvore AVL
    public Node InsertAVL(int v){
        Node n = Insert(v);
        Node i = n; // Nó para subir na árvore e atualizar os fatores de balanceamento

        while(n.Pai != null){
            // Atualiza o fator de balanceamento
            if(n.Pai.FilhoEsquerdo == n){
                n.Pai.FB++;
            } else{
                n.Pai.FB--;
            }

            // Verifica se desbalanceou
            if(Math.Abs(n.Pai.FB) > 1){
                Rebalance(n);
                break;
            }
            n = n.Pai;
        }
        return i;
    }


    public Node RemoveAVL(int v){
        Node n = Remove(v);
        Node r = n; // Nó para subir na árvore e atualizar os fatores de balanceamento

        while(n.Pai != null){
            // Atualiza o fator de balanceamento
            n.Pai.FB = Height(n.Pai.FilhoEsquerdo) - Height(n.Pai.FilhoDireito);
        
            // Verifica se desbalanceou
            if(Math.Abs(n.Pai.FB) > 1){
                Rebalance(n);
                break;
            }
            n = n.Pai;
        }
        return r;
    }


    private void Rebalance(Node n){
        bool negativo = (IsPositive(n.Pai.FB) && !IsPositive(n.FB) || (!IsPositive(n.Pai.FB) && IsPositive(n.FB)));
        bool positivo = IsPositive(n.Pai.FB);

        // Rotação Esquerda Simples
        if(!positivo && !negativo){
            LeftRotation(n.Pai, n);
        }

        // Rotação Direita Simples
        if(positivo && !negativo){
            RightRotation(n.Pai, n);
        }

        // Rotação Esquerda Dupla
        if(!positivo && negativo){
            RightRotation(n, n.FilhoEsquerdo);
            LeftRotation(n.Pai.Pai, n.Pai);
        }

        // Rotação Direita Dupla
        if(positivo && negativo){
            LeftRotation(n, n.FilhoDireito);
            RightRotation(n.Pai.Pai, n.Pai);
        }
    }


    // Rotação Esquerda
    private void LeftRotation(Node p, Node n){
        Node lc = null; // Filho esquerdo de n
        Node rc = null; // Filho direito de n

        if(n != null){
            lc = n.FilhoEsquerdo;
            rc = n.FilhoDireito;
        }

        // Avô de n vira pai de n
        if(p == Raiz){
            Raiz = n;
            n.Pai = null;
        } else{
            Node avo = p.Pai;
            n.Pai = avo;

            if(avo.FilhoEsquerdo == p){
                avo.FilhoEsquerdo = n;
            } else{
                avo.FilhoDireito = n;
            }
        }

        // Filho esquerdo de n vira filho direito de pai
        if(lc == null){
            p.FilhoDireito = lc;
        } else{
            p.FilhoDireito = lc;
            lc.Pai = p;
        }

        // Pai vira filho esquerdo de n
        n.FilhoEsquerdo = p;
        p.Pai = n;

        // Atualiza fatores de balanceamento
        p.FB = p.FB + 1 - Math.Min(n.FB, 0);
        n.FB = n.FB + 1 + Math.Max(p.FB, 0);
    }


    // Rotação Direita
    private void RightRotation(Node p, Node n){
        Node lc = n.FilhoEsquerdo; // Filho esquerdo de n
        Node rc = n.FilhoDireito; // Filho direito de n

        // Avô de n vira pai de n
        if(p == Raiz){
            Raiz = n;
            n.Pai = null;
        } else{
            Node avo = p.Pai;
            n.Pai = avo;

            if(avo.FilhoEsquerdo == p){
                avo.FilhoEsquerdo = n;
            } else{
                avo.FilhoDireito = n;
            }
        }

        // Filho direito de n vira filho esquerdo de pai
        if(rc == null){
            p.FilhoEsquerdo = rc;
        } else{
            p.FilhoEsquerdo = rc;
            rc.Pai = p;
        }

        // Pai vira filho direito de n
        n.FilhoDireito = p;
        p.Pai = n;

        // Atualiza fatores de balanceamento
        p.FB = p.FB - 1 - Math.Max(n.FB, 0);
        n.FB = n.FB - 1 + Math.Min(p.FB, 0);
    }


    // Verifica se o Valor é Positivo
    private bool IsPositive(int v){
        if(v > 0){
            return true;
        } else{
            return false;
        }
    }


    // ===== Métodos de Impressão =====
    public void PrintElements(){
        if(tamanho == 0){
            Console.WriteLine("Árvore vazia");
        }

        Console.Write("(");
        PrintElement(Raiz);
        Console.WriteLine(")");
    }


    // Imprime os Elementos em Ordem
    public void PrintElement(Node n){
        if(n == null){
            return;
        }

        if(IsInternal(n)){
            PrintElement(n.FilhoEsquerdo);
        }

        Console.Write(" " + n.Valor + " ");

        if(IsInternal(n)){
            PrintElement(n.FilhoDireito);
        }
    }


    // Imprime a Árvore
    public void PrintTree(){
        if(tamanho == 0){
            Console.WriteLine("Árvore vazia");
        }

        elementos = new ArrayList();
        SetElements(Raiz);
        int [,] matriz = new int[Height(Raiz) + 1, tamanho];
        string [,] matrizFB = new string[Height(Raiz) + 1, tamanho];
        SetMatriz(matriz, matrizFB, elementos);
        PrintMatriz(matriz, matrizFB, Height(Raiz) + 1, tamanho);
    }


    // Imprime a Matriz
    public void PrintMatriz(int[,] m, string[,] mfb, int x, int y){
        for(int i = 0; i < x; i++){
            for(int j = 0; j < y; j++){
                if(m[i, j] == 0){
                    Console.Write("   ");
                } else{
                    Console.Write(""+m[i, j]+"⁽"+mfb[i, j]+"⁾");
                }
            }
            Console.WriteLine();
        }
    }


    // Preenche a Matriz com os Valores dos Nós e seus Fatores de Balanceamento
    public void SetMatriz(int[,] m, string[,] mfb, ArrayList elementos){
        int i = 0;
        for(i = 0; i < Height(Raiz); i++){
            for(int j = 0; j < tamanho; j++){
                m[i, j] = 0;
            }
        }

        // Preenchendo a matriz com os valores dos nós
        i = 0;
        foreach(object obj in elementos){
            m[Depth((Node)obj), i] = ((Node)obj).Valor;
            mfb[Depth((Node)obj), i] = MiniFB(((Node)obj).FB);
            i++;
        }
    }


    // Preenche o ArrayList com os Elementos da Árvore em Ordem
    private void SetElements(Node n){
        if(IsInternal(n) && n.FilhoEsquerdo != null){
            SetElements(n.FilhoEsquerdo);
        }

        elementos.Add(n);

        if(IsInternal(n) && n.FilhoDireito != null){
            SetElements(n.FilhoDireito);
        }
    }


    // Retorna o Fator de Balanceamento
    private string MiniFB(int v){
        string fb = "";
            if (v == 0) fb = "⁰";
            else if (v == 1) fb = "¹";
            else if (v == -1) fb = "⁻¹";
            else if (v == 2) fb = "²";
            else if (v == -2) fb = "⁻²";

            return fb;
    }


    // Construtor
    public ArvoreAVL(int v){
        Node r = new Node(null, v);
        Raiz = r;
        tamanho++;
    }
}