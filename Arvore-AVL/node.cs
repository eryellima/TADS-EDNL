public class Node{
    public int Valor { get; set; }
    
    public int FB { get; set; }
    public Node? Pai { get; set; }
    public Node? FilhoEsquerdo { get; set; }
    public Node? FilhoDireito { get; set; }

    public Node(Node p, int v){
        this.Valor = v;
        this.FB = 0;
        this.Pai = p;
        this.FilhoEsquerdo = null;
        this.FilhoDireito = null;
    }

    public override string ToString(){
        return $"Elemento = {Valor}, FB = {FB}";
    }
}