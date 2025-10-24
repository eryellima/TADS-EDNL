using System.Collections;
using System.Diagnostics.Tracing;

public class ArvoreAVL{
    private int tamanho = 0;
    private No? raiz;
    private ArrayList elementos = new ArrayList();

    public int Tamanho{
        get { return tamanho; }
    }

    public No? Raiz{
        get { return raiz; }
    }

    public bool isRoot(No n){
        return raiz == n;
    }

    public isExternal(No n){
        return n.FilhoEsquerdo == null && n.FilhoDireito == null;
    }

    public isInternal(No n){
        return !isExternal(n);
    }

    public int Profundidade(No n){
        if(isRoot(n)){
            return 0;
        } else {
            return 1 + Profundidade(n.Pai);
        }
    }


    public int Altura(No n){
        if(n is null || isExternal(n)) return 0;
        return 1 + Math.Max(Altura(n.FilhoEsquerdo), Altura(n.FilhoDireito));
    }

    public No Busca(int v){
        return Busca(raiz, v);
    }

    private No Busca(No? n, int v){
        // Verifica se é externo
        if(isExternal(n)){
            return n;
        }

        // Se o valor for menor -> caminha para esquerda
        if(v < n.Valor){
            if(n.FilhoEsquerdo != null)
                return Busca(n.FilhoEsquerdo, v);
        }

        // Se o valor for maior -> caminha para direita
        else if(v > n.Valor){
            if(n.FilhoDireito != null)
                return Busca(n.FilhoDireito, v);
        }

        // Valor igual -> retorna o nó
        return n;
    }


    private No Inserir(int v){
        // Verificando se a árvore está vazia
        if(raiz == null){
            raiz = new No(null, v);
            return raiz;
        }

        No n = Busca(v);
        
        if(n.Valor == v){
            throw new Exception("Valor já existe na árvore");
        }
        
        No novoNo = new No(n, v);
        
        if(v < n.Valor){
            n.FilhoEsquerdo = novoNo;
        }
        
        if(v > n.Valor){
            n.FilhoDireito = novoNo;
        }

        tamanho++;
        return novoNo;
    }

    private No Remover(No v){
        No n = RemoverNo(v);
        tamanho--;
        return n;
    }

    private No RemoverNo(int v){
        No n = Busca(v);

        if(n.Valor != v) throw new Exception("Valor não encontrado na árvore");

        int type = CaseType(n);

        switch(type){
            case 0:
                RemoveRaiz();
                break;

            case 1:
                RemoveFolha(n);
                break;

            case 2:
                RemoveNoComUmFilho(n);
                break;

            case 3:
                RemoveNoComDoisFilhos(n);
                break;
        }

        return n;
    }


    private int CaseType(No n){
        // caso 0: raiz é o unico nó
        if(n == raiz && isExternal(n)){
            return 0;
        }

        // caso 1: o nó é folha
        if(isExternal(n)){
            return 1;
        }

        // caso 2: o nó tem um filho
        if(n.FilhoEsquerdo != null && n.FilhoDireito != null){
            return 2;
        }

        // caso 3: o nó tem dois filhos
        if(n.FilhoEsquerdo != null || n.FilhoDireito != null){
            return 3;
        }

        return -1; // caso não encontrado
    }


    private void RemoveRaiz(){
        raiz = null;
    }


    private void RemoveFolha(No n){
        No o = n.Pai;
        if(n == o.FilhoEsquerdo){
            o.FilhoEsquerdo = null;
        } else{
            o.FilhoDireito = null;
        }
    }


    private void RemoveNoComUmFilho(No n){
        No o = n.Pai;
        No p;

        // Verifica em qual lado o filho de n está
        if(n.FilhoEsquerdo != null)
    }
}