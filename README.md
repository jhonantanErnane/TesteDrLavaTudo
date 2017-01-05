# TesteDrLavaTudo

1. Quais alterações devemos fazer nessa estrutura para que o cliente consigo fazer mais de um serviço por solicitação?

Cliente            | Serviços                        | Ordem de Serviço           | Itens da Ordem de serviço
------------------ | ------------------------------- | -------------------------- |---------------------------------
ID Cliente         | ID Serviço (auto Incrimentável) | ID OS (auto Incrimentável) | ID Itens OS (auto Incrimentável)
Nome               | Nome Serviço                    | ID Cliente                 | ID Serviço
E-mail             | Valor Final                     | Data Emissão               | ID OS
Data de Nascimento | Custo Empresa                   |                            | Data Contratação
Tel. Cel           |                                 |                            | Data Execução
Tel. Res           |

2. E se a mesma ordem de serviço tivesse serviços para endereços diferentes. Como ficaria a nova estrutura de dados?

Cliente            | Serviços                        | Ordem de Serviço           | Itens da Ordem de serviço        | Endereço
------------------ | ------------------------------- | -------------------------- |--------------------------------- |----------
ID Cliente         | ID Serviço (auto Incrimentável) | ID OS (auto Incrimentável) | ID Itens OS (auto Incrimentável) | CEP(ID)
Nome               | Nome Serviço                    | ID Cliente                 | ID Serviço                       | Rua
E-mail             | Valor Final                     | Data Emissão               | ID OS                            | Bairro
Data de Nascimento | Custo Empresa                   |                            | Data Contratação                 | Cidade
Tel. Cel           |                                 |                            | Data Execução                    | UF
Tel. Res           |                                 |                            | CEP                              |
CEP                |                                 |                            | Numero                           |
Numero             |                                 |                            |

3. Utilizando qualquer Linguagem de Consulta Estruturada (SQL) e considerando a nova estrutura de dados criada na questão anterior:

a. Selecione todos os clientes e a quantidade de ordem de serviços

SELECT CLI.NOME,COUNT(OS.IDOS) 'Quantidade de OS por cliente'	
FROM CLIENTE CLI	
INNER JOIN OS ON CLI.IDCLIENTE = OS.IDCLIENTE	
GROUP BY CLI.NOME	


b. Selecione todas as Ordens de Serviços com mais de um serviço

SELECT DISTINCT OS.IDOS	    
FROM OS	    
INNER JOIN OSITENS IT ON IT.IDOS = OS.IDOS	
WHERE	   
IT.IDSERVICO >1	     

c. Selecione os serviços mais vendidos

SELECT DISTINCT S.NOME ,COUNT(IT.IDSERVICO) QUANTIDADE       
FROM OSITENS IT          
INNER JOIN SERVICO S ON S.IDSERVICO=IT.IDSERVICO         
GROUP BY S.NOME        
ORDER BY QUANTIDADE DESC       

d. Atualize o valor final de todos os serviços em 12%

UPDATE SERVICO SET VALORFINAL = VALORFINAL*1.12

e. Remova a ultima ordem de serviço criada

DECLARE @OS INTEGER,@ITOS INTEGER         
SET @OS= (SELECT MAX(IDOS) FROM OS)         
SET @ITOS = (SELECT MAX(IDOS) FROM OSITENS)         
IF @OS=@ITOS         
BEGIN         
DELETE FROM OSITENS WHERE @ITOS = IDOS          
DELETE FROM OS WHERE @OS = IDOS          
END         
ELSE         
DELETE FROM OS WHERE @OS = IDOS ;         

f. Insira um cliente

INSERT INTO CLIENTE VALUES(3,'bruno@live.com','1993-09-23','Bruno Couto',31999346578,3125567654,987,30345123)
