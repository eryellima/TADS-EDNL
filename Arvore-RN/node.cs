namespace ArvoreRubroNegra{
    public class NodeRN{
        private int valor; // valor do nó

        private int fb; // fator de balanceamento
        private char cor; // 'R' para vermelho, 'N' para negro
        private NodeRN? pai; // nó pai
        private NodeRN? filhoEsquerdo; // nó filho esquerdo
        private NodeRN? filhoDireito; // nó filho direito

        public int Valor{
            get { return valor; }
            set { valor = value; }
        }
        
        public int FB{
            get { return fb; }
            set { fb = value; }
        }

        public char Cor{
            get { return cor; }
            set { cor = value; }
        }

        public NodeRN Pai{
            get { return pai; }
            set { pai = value; }
        }

        public NodeRN FilhoEsquerdo{
            get { return filhoEsquerdo; }
            set { filhoEsquerdo = value; }
        }

        public NodeRN FilhoDireito{
            get { return filhoDireito; }
            set { filhoDireito = value; }
        }

        // Construtor
        public NodeRN(NodeRN p, int v, char c = 'R'){
            valor = v;
            pai = p;
            fb = 0;
            filhoEsquerdo = null;
            filhoDireito = null;
            cor = c;
        }

        // Método para representar o nó como string
        public override string ToString(){
            return $"Elemento = {valor}, Cor = {cor}";  
        }
    }
}