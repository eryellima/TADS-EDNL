public class No{
    public int Valor { get; set; }
    public int FB { get; set; }
    public No? Pai { get; set; }
    public No? FilhoEsquerdo { get; set; }
    public No? FilhoDireito { get; set; }

    public No(No? p, int v){
        Valor = v;
        Pai = p;
        FB = 0;
        FilhoEsquerdo = null;
        FilhoDireito = null;
    }
}