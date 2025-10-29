using System.Collections;


namespace ArvoreRubroNegra{
    public class ArvoreRN{
        private int tamanho = 0;    // número de nós na árvore
        private NodeRN raiz;    // raiz da árvore
        private ArrayList elementos = new ArrayList();  // lista de elementos na árvore


        // ===== MÉTODOS DE ABP e AVL =====
        public int Size(){
            return tamanho;
        }


        public NodeRN Root(){
            return raiz;
        }


        public bool IsRoot(NodeRN n){
            return raiz == n;
        }


        public bool IsExternal(NodeRN n){
            return (n.FilhoEsquerdo == null) && (n.FilhoDireito == null);
        }


        public bool IsInternal(NodeRN n){
            return !IsExternal(n);
        }


        public int Depth(NodeRN n){
            if (IsRoot(n)){
                return 0;
            } else {
                // Profundidade é 1 + Profundidade do Pai
                return 1 + Depth(n.Pai);
            }
        }


        public int Height(NodeRN n){
            if(n == null || IsExternal(n)){
                return 0;
            } else {
                // Altura é 1 + Altura Máxima dos Filhos
                return 1 + Math.Max(Height(n.FilhoEsquerdo), Height(n.FilhoDireito));
            }
        }


        public NodeRN Search(int v){
            return SearchAux(raiz, v);
        }


        public NodeRN SearchAux(NodeRN n, int v){
            if(IsExternal(n)){
                return n;
            }

            // Se o Valor é Menor, Vai Para a Subárvore Esquerda
            if(v < n.Valor){
                if(n.FilhoEsquerdo != null){
                    return SearchAux(n.FilhoEsquerdo, v);
                }
            }

            // Se o Valor é Maior, Vai Para a Subárvore Direita
            if(v > n.Valor){
                if(n.FilhoDireito != null){
                    return SearchAux(n.FilhoDireito, v);
                }
            }
            return n;
        }


        public NodeRN Insert(int v){
            if(raiz == null){
                raiz = new NodeRN(null, v);
                return raiz;
            }

            NodeRN n = Search(v);   // procura a posição para inserir o novo nó
            NodeRN novoNo = new NodeRN(n, v);   // cria o novo nó

            if(v < n.Valor){
                n.FilhoEsquerdo = novoNo;
            }
            
            if(v > n.Valor){
                n.FilhoDireito = novoNo;
            }
            tamanho++; // incrementa o tamanho
            return novoNo;
        }


        public NodeRN Remove(int v){
            NodeRN n = RemoveAux(v);
            tamanho--;
            return n;
        }


        public NodeRN RemoveAux(int v){
            NodeRN n = Search(v);

            if(n.Valor != v){
                throw new Exception("Valor não encontrado na árvore.");
            }

            int tipo = CaseType(n); // o tipo de remoção

            switch(tipo){
                // caso 0: nó é a raiz
                case 0:
                    RemoveRoot();
                    break;
                
                // caso 1: nó é externo (folha)
                case 1:
                    RemvoveLeaf(n);
                    break;

                // caso 2: nó tem um filho
                case 2:
                    RemoveOneChild(n);
                    break;

                // caso 3: nó tem dois filhos
                case 3:
                    RemoveTwoChild(n);
                    break;
            }
            return n;
        }


        private int CaseType(NodeRN n){
            // caso 0: nó é a raiz
            if(IsRoot(n)){
                return 0;
            }

            // caso 1: nó é um folha
            if(IsExternal(n)){
                return 1;
            }

            // caso 2: nó tem um filho
            if((n.FilhoEsquerdo != null) || (n.FilhoDireito != null)){
                return 2;
            }

            // caso 3: nó tem dois filhos
            if((n.FilhoEsquerdo != null) && (n.FilhoDireito != null)){
                return 3;
            }
            return -1; // erro
        }


        private void RemoveRoot(){
            raiz = null;
        }


        private void RemvoveLeaf(NodeRN n){
            NodeRN p = n.Pai; // nó pai

            // Remove a referência do pai do nó que vai ser removido
            if(n == p.FilhoEsquerdo){
                p.FilhoEsquerdo = null;
            } else {
                p.FilhoDireito = null;
            }
        }


        private void RemoveOneChild(NodeRN n){
            NodeRN p = n.Pai; // nó pai
            NodeRN f; // nó filho

            // Verifica de que lado está o filho
            if(n.FilhoEsquerdo != null){
                f = n.FilhoEsquerdo;
            } else {
                f = n.FilhoDireito;
            }

            // Remove a referência do pai do nó que será removido
            if(n == p.FilhoEsquerdo){
                p.FilhoEsquerdo = f;
                f.Pai = p;
            } else {
                p.FilhoDireito = f;
                f.Pai = p;
            }
        }


        private void RemoveTwoChild(NodeRN n){
            NodeRN s = FindSucessor(n.FilhoDireito); // sucessor do nó removido

            // troca o valor do nó removido com o valor do sucessor
            if(s != null){
                int temp = s.Valor;
                RemoveAux(temp);
                n.Valor = temp;
            }
        }



        public NodeRN FindSucessor(NodeRN n){
            // o sucessor é o nó mais à esquerda da subárvore direita
            if(n.FilhoEsquerdo != null){
                return FindSucessor(n.FilhoEsquerdo);
            }
            return n;
        }


        // ===== MÉTODOS DE ÁRVORE RUBRO-NEGRA =====
        public NodeRN InsertRN(int v){
            NodeRN n = Insert(v); // insere o nó como em uma ABP normal
            n.FilhoEsquerdo = new NodeRN(n, 0, 'N'); // cria o filho esquerdo negro
            n.FilhoDireito = new NodeRN(n, 0, 'N'); // cria o filho direito negro

            NodeRN tio = GetUncle(n);

            while (!IsRoot(n)){
                // Duplo Rubro
                if(n.Cor == 'R' && n.Pai.Cor == 'R'){
                    InsertBalance(n, tio, n.Pai, n.Pai.Pai);
                }
                n = n.Pai;  // sobe na árvore
            }

            raiz.Cor = 'N';     // garantindo que a raiz seja sempre negra
            return n;
        }


        private void InsertBalance(NodeRN n, NodeRN t, NodeRN p, NodeRN a){
            // Caso 1: Tio é vermelho
            if (t.Cor == 'R'){
                p.Cor = 'N';    // pai
                t.Cor = 'N';    // tio
                a.Cor = 'R';    // avô

                if (!IsRoot(a) && a.Cor == 'R' && a.Pai.Cor == 'R'){
                    InsertBalance(a, GetUncle(a), a.Pai, a.Pai.Pai);
                }
            } else{
                BalanceRotation(n, p, a);
            }
        }


        private void BalanceRotation(NodeRN n, NodeRN p, NodeRN a){
            // Rotação Esquerda Simples
            if ((a.FilhoDireito == p) && (p.FilhoDireito == n)){
                LeftRotation(p, n);
            }

            // Rotação Direita Simples
            if ((a.FilhoEsquerdo == p) && (p.FilhoEsquerdo == n)){
                RightRotation(p, n);
            }
            
            // Rotação Esquerda Dupla
            if ((a.FilhoEsquerdo == p) && (p.FilhoDireito == n)){
                RightRotation(n, n.FilhoEsquerdo);
                LeftRotation(a, p);
            }
            
            // Rotação Direita Dupla
            if ((a.FilhoDireito == p) && (p.FilhoEsquerdo == n)){
                LeftRotation(n, n.FilhoDireito);
                RightRotation(a, p);
            }
        }


        private void LeftRotation(NodeRN p, NodeRN n){
            NodeRN lc = null;
            NodeRN rc = null;

            if (n != null){
                lc = n.FilhoEsquerdo;
                rc = n.FilhoDireito;
            }

            // Avô vira pai do no
            if (p == raiz){
                raiz = n;
                n.Pai = null;
            } else{
                NodeRN avo = p.Pai;
                n.Pai = avo;

                if (avo.FilhoEsquerdo == p){
                    avo.FilhoEsquerdo = n;
                } else{
                    avo.FilhoDireito = n;
                }
            }

            // Filho esquerdo do no vira filho direito do pai
            if (lc == null){
                p.FilhoDireito = lc;
            } else{
                p.FilhoDireito = lc;
                lc.Pai = p;
            }

            // Pai vira filho esquerdo
            n.FilhoEsquerdo = p;
            p.Pai = n;

            // Atualiza o Fator de Balanceamento
            p.FB = p.FB + 1 - Math.Min(n.FB, 0);
            n.FB = n.FB + 1 + Math.Max(p.FB, 0);
        }


        private void RightRotation(NodeRN p, NodeRN n){
            NodeRN lc = n.FilhoEsquerdo;
            NodeRN rc = n.FilhoDireito;

            // Avô vira pai do no
            if (p == raiz){
                raiz = n;
                n.Pai = null;
            } else{
                NodeRN avo = p.Pai;
                n.Pai = avo;
                if (avo.FilhoEsquerdo == p){
                    avo.FilhoEsquerdo = n;
                } else {
                    avo.FilhoDireito = n;}
            }

            // Filho direito do no vira filho esquerdo do pai
            if (rc == null){
                p.FilhoEsquerdo = rc;
            } else{
                p.FilhoEsquerdo = rc;
                rc.Pai = p;
            }

            // Pai vira filho direito
            n.FilhoDireito = p;
            p.Pai = n;

            // Atualizando Fator de Balanceamento
            p.FB = p.FB - 1 - Math.Max(n.FB, 0);
            n.FB = n.FB - 1 + Math.Min(p.FB, 0);
        }


        private NodeRN GetUncle(NodeRN n){
            if (n.Pai.FilhoEsquerdo == n){
                return n.Pai.FilhoDireito;
            } else{
                return n.Pai.FilhoEsquerdo;
            }
        }
        

        // ===== MÉTODOS DE IMPRESSÃO =====
        public void PrintElements () {
            if (tamanho == 0){
                throw new Exception("A árvore está Vazia");
            }
            
            Console.Write("(");
            PrintElement(raiz);
            Console.WriteLine(")");
        }


        private void PrintElement(NodeRN n){
            if (n == null){
                return;
            }

            if (IsInternal(n)){
                PrintElement(n.FilhoEsquerdo);
            }

            Console.Write(" " + n.Valor + " ");

            if (IsInternal(n)){
                PrintElement(n.FilhoDireito);
            }
        }
        

        public void PrintTree () {
            if (tamanho == 0){
                throw new Exception("A árvore está  vazia");
            }

            elementos = new ArrayList();
            SetElements(raiz);
            
            int [,] matriz = new int[Height(raiz) + 1, tamanho];
            string [,] matrizColor = new string[Height(raiz) + 1, tamanho];
            SetMatriz(matriz, matrizColor, elementos);
            PrintMatriz(matriz, matrizColor, Height(raiz) + 1, tamanho);
        }


        private void PrintMatriz(int[,] m, string[,] mc, int x, int y){
            for (int i = 0; i < x; i++){
                for (int j = 0; j < y; j++){
                    if (m[i, j] == 0){
                        Console.Write("   ");
                    } else{
                        Console.Write("" + m[i, j] + mc[i, j]);
                    }
                }
                Console.WriteLine();
            }
        }
        

        private void SetMatriz (int[,] m, string[,] mc, ArrayList elementos) {
            int i = 0;

            for (i = 0; i < Height(raiz); i++) {
                for (int j = 0; j < tamanho; j++) {
                    m[i, j] = 0;
                }
            }

            //Atribui elementos a martiz
            i = 0;
            foreach (object obj in elementos) {
                m[Depth((NodeRN)obj), i] = ((NodeRN)obj).Valor;
                mc[Depth((NodeRN)obj), i] = CharCor(((NodeRN)obj).Cor);
                i++;
            }
        }


        private void SetElements(NodeRN n){
            if (IsInternal(n) && n.FilhoEsquerdo != null){
                SetElements(n.FilhoEsquerdo);
            }

            elementos.Add(n);
            
            if (IsInternal(n) && n.FilhoDireito != null) {
                SetElements(n.FilhoDireito);
            }
        }


        private string CharCor(char c){
            string cor = "";


            if (c == 'R'){
                cor = "ᴿ";
            } else if (c == 'B'){
                cor = "ᴮ";
            }

            return cor;
        }
        
        
        // Construtor
        public ArvoreRN (int v) {
            NodeRN rn = new NodeRN(null, v);
            raiz = rn;
            tamanho++;
        }
    }
}