# Árvores AVL
Criada em 1962 por Adel'son-Vel'skii e  Landis (daí o nome AVL), é uma árvore binária de busca que se autoajusta.
A cada inserção ou remoção, ela verifica se continua balanceada.
Se não estiver, ela faz rotações (pequenas reorganizações) para recuperar o equilibrio.

## Definição
Uma árvore binária `T` é AVL qunado, para todo nó `v`:
> he(v)−hd(v)∣≤1

Onde:
- he(v) = altura da subárvore esquerda
- hd(v) = altura da subárvore direita

> Ou seja, a diferença entre as alturas dos dois lados de qualquer nó é, no máximo, 1.

## Fator de Balanceamento (FB)
Cada nó da árvore guarda (ou pode calcular) um número chamado Fator de balanceamento (FB):
> FB(v) = he(v) - hd(v)

Esse número mostra "qual lado está pesando mais".

| Valor de FB | Significado                       | Situação
|:-----------:|-----------------------------------|:--------:
| +1          | Lado esquerdo é mais alto         | Ok
| 0           | Ambos os lados têm a mesma altura | Ok
| -1          | Lado direito é mais alto          | Ok
| >1          | Muito pesado à esquerda           | Desbalanceado
| < -1        | Muito pesado à direita            | Desbalanceado

### Quando o desbalanceamento ocorre?
- Se inserirmos algo na subárvore esquerda, o FB aumenta em +1.
- Se inserirmos algo na subárvore direita, o FB diminui em -1.
- Na remoção acontece o oposto.

| Operação | Subárvore Esqeruda | Subárvore Direita
|----------|:------------------:|:-----------------:
| Inserção | +1                 | -1
| Remoção  | -1                 | +1

Durante a inserção, se o FB de um nó "subir" para fora do intervalo [-1, 1], precisamos rebalancear.

## Rotações
Uma rotação é uma troca de posições entre nós para "reorganizar" a árvore sem perder a ordem da BST (o percurso em-ordem continua o mesmo).

Existem 4 tipos de rotações:
Tipo                             | Quando usar                         | O que faz
---------------------------------|-------------------------------------|----------
Rotação Simples à Direita (RSD)  | FB = +2 e FB do filho esquerdo >= 0 | Move o nó da esquerda para cima
Rotação Simples à Esquerda (RSE) | FB = -2 e FB do filho direito <= 0  | Move o nó da direita para cima
Rotação Dupla à Direita          | FB = +2 e FB do filho esquerdo < 0  | Corrige um "cotovelo" à esquerda
Rotação Dupla à Esquerda         | FB = -2 e FB do filho direito > 0   | Corrige um "cotovelo" à direita

### Como Funcionam as Rotações
**Rotação Simples à Dreita (RSD)**
Usado quando a árvore ficou mais alta à esquerda.
~~~yaml
    A
   /
  B
 /
C
~~~

Após a rotação à direita:
~~~yaml
    B
   / \
  C   A
~~~

**Rotação Simples à Esquerda (RSE)**
É o inverso da anterior. Usada quando a árvore ficou mais alta à direita.
~~~yaml
A
 \
  B
   \
    C
~~~

Após a rotação à esquerda:
~~~yaml
   B
  / \
 A   C
~~~

**Rotação Dupla à Direita (RDD)**
Usada quando a árvore tem uma forma de "cotovelo" à esquerda:
~~~yaml
    A
   /
  B
   \
    C
~~~

Passos:
1. Rotação simples à esquerda em `B`
2. Rotação simples à direita em `A`

Resultado final:
~~~yaml
    C
   / \
  B   A
~~~

**Rotação Dupla à Esquerda (RED)**
Semelhante à anterior:
~~~yaml
A
 \
  B
 /
C
~~~

1. Rotação simples à direita em `B`
2. Rotação simples à esquerda em `A`

Resultado:
~~~yaml
   C
  / \
 A   B
~~~

### Atualizando o FB após Rotações
Quando rotacionamos, os fatores de balanceamento dos nós envolvidos (geralmente dois ou três) precisam ser recalculados.
- Rotação à Esquerda:
~~~cs
FB_B_novo = FB_B + 1 - min(FB_A, 0)
FB_A_novo = FB_A + 1 + max(FB_B_novo, 0)
~~~

- Rotação à Direita:
~~~cs
FB_B_novo = FB_B - 1 - max(FB_A, 0)
FB_A_novo = FB_A - 1 + min(FB_B_novo, 0)
~~~

Essas fórmulas garantem que o novo balanceamento fique dentro de [-1, 1].

## Inserção em Árvore AVL
1. Faz a busca normal como em uma árvore binária de pesquisa
2. Insere o novo nó como folha.
3. Atualiza as alturas/FB ao subir a árvore
4. Se algum nó ficar com o FB fora do intervalo [-1, 1], faz a rotação adequada.

## Remoção em Árvores AVL
1. Busca o nó normalmente (como em uma ABP/BST)
2. Remove conforme o caso (sem filsho, um filho ou dois filhos).
3. Atualiza os FB dos ancestrais
4. Se algum nó desbalancear, faz a rotação necessária.
