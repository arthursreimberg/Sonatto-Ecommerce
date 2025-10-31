-- Tabelas do Ecommerce
DROP DATABASE dbSonatto;
CREATE DATABASE dbSonatto;
USE dbSonatto;

-- Tabela de Usuario
CREATE TABLE tbUsuario (
    IdUsuario INT PRIMARY KEY AUTO_INCREMENT,
    Email VARCHAR(50) NOT NULL,
    Nome VARCHAR(100) NOT NULL,
    Senha VARCHAR(100) NOT NULL,
    CPF VARCHAR(11) NOT NULL UNIQUE,
    Endereco VARCHAR(150) NOT NULL,
    Telefone VARCHAR(11) NOT NULL
);

-- Tabela Nivel de acesso
CREATE TABLE tbNivelAcesso(
	idNivel INT PRIMARY KEY AUTO_INCREMENT,
    NomeNivel VARCHAR(50) NOT NULL
);
INSERT INTO tbNivelAcesso(NomeNivel)
VALUES
	('Nivel l'),
	('Nivel 2'),
	('Nivel 3');

-- Tabela Nivel de referenciamento Nivel de acesso
CREATE TABLE tbUsuNivel(
	IdUsuario INT,
    IdNivel INT,
    PRIMARY KEY (IdUsuario, IdNivel),
  	CONSTRAINT fk_IdUsuario FOREIGN KEY(IdUsuario) REFERENCES tbUsuario(IdUsuario),
    CONSTRAINT fk_IdNivel FOREIGN KEY(IdNivel) REFERENCES tbNivelAcesso(IdNivel)
);

-- Tabela de Produto
CREATE TABLE tbProduto(
    IdProduto INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
    NomeProduto VARCHAR(100) NOT NULL,
    Descricao VARCHAR(2500) NOT NULL,
    Preco DECIMAL(8,2) NOT NULL CHECK(Preco > 0),
	Marca VARCHAR(100) NOT NULL,
    Categoria VARCHAR(100) NOT NULL,
    Avaliacao DECIMAL (2,1) NOT NULL,
    EstadoProduto BIT NOT NULL
);

-- Tabela de Imagens dos Produtos
CREATE TABLE tbImagens(
	IdImagem INT AUTO_INCREMENT PRIMARY KEY,
    IdProduto INT,
    UrlImagem varchar(255),
    CONSTRAINT fk_ImgIdProduto FOREIGN KEY(IdProduto) REFERENCES tbProduto(IdProduto)
);

-- Tabela de Estoque
CREATE TABLE tbEstoque(
    IdEstoque INT PRIMARY KEY AUTO_INCREMENT,
    IdProduto INT NOT NULL,
    QtdEstoque INT NOT NULL,
    Disponibilidade BIT NOT NULL,
    CONSTRAINT fk_IdEstoque_IdProduto FOREIGN KEY(IdProduto) REFERENCES tbProduto(IdProduto)
);


-- Tabela de Venda
CREATE TABLE tbVenda(
    IdVenda INT PRIMARY KEY AUTO_INCREMENT,
    IdUsuario INT NOT NULL,
    TipoPag VARCHAR(50) NOT NULL,
    QtdTotal INT NOT NULL,
    ValorTotal DECIMAL(8,2) NOT NULL,
    CONSTRAINT fk_idVenda_IdUsuario FOREIGN KEY(IdUsuario) REFERENCES tbUsuario(IdUsuario)
);

-- Tabela de ItemVenda
CREATE TABLE tbItemVenda(
    IdVenda INT AUTO_INCREMENT NOT NULL,
    IdProduto INT NOT NULL,
    PrecoUni DECIMAL(8,2),
    Qtd INT NOT NULL,
    PRIMARY KEY(IdVenda, IdProduto),
    CONSTRAINT fk_IdItemVenda_IdVenda FOREIGN KEY(IdVenda) REFERENCES tbVenda(IdVenda),
    CONSTRAINT fk_IdItemVenda_IdProduto FOREIGN KEY(IdProduto) REFERENCES tbProduto(IdProduto)
);

-- Tabela de Nota Fiscal
CREATE TABLE tbNotaFiscal(
    NumNotaFiscal INT PRIMARY KEY AUTO_INCREMENT,
    IdVenda INT NOT NULL UNIQUE,
    DataEmissao DATE NOT NULL,
    Numero VARCHAR(20) NOT NULL UNIQUE,
    PrecoTotal DECIMAL(8,2) NOT NULL,
    CONSTRAINT fk_IdNotaFiscal_IdVenda FOREIGN KEY(IdVenda) REFERENCES tbVenda(IdVenda)
);

CREATE TABLE tbCarrinho(
	IdCarrinho INT PRIMARY KEY AUTO_INCREMENT,
	IdUsuario INT NOT NULL,
	DataCriacao DATE NOT NULL,
    Estado BIT NOT NULL,
    ValorTotal DECIMAL(8,2) NOT NULL,
    FOREIGN KEY (IdUsuario) REFERENCES tbUsuario(IdUsuario)
);

CREATE TABLE tbItemCarrinho(
	IdItemCarrinho INT PRIMARY KEY AUTO_INCREMENT,
    IdCarrinho INT NOT NULL,
    IdProduto INT NOT NULL,
    QtdItemCar INT NOT NULL,
    PrecoUnidadeCar DECIMAL(8,2) NOT NULL,
    SubTotal DECIMAL(8,2) NOT NULL,
    FOREIGN KEY (IdCarrinho) REFERENCES tbCarrinho(IdCarrinho),
    FOREIGN KEY (IdProduto) REFERENCES tbProduto(IdProduto)
);


-- Procedures 

-- IN => Valor de entrada
-- OUT => Valor de saída
-- procedure para criar usuario
-- drop procedure sp_CadastroUsu
DELIMITER $$
CREATE PROCEDURE sp_CadastroUsu(
    IN vEmail VARCHAR(50),
    IN vNome VARCHAR(100),
    IN vSenha VARCHAR(100),
    IN vCPF VARCHAR(11),
    IN vEndereco VARCHAR(150),
    IN vTelefone VARCHAR(11),
    OUT vIdCli INT
)
BEGIN
	INSERT INTO tbUsuario (Email, Nome, Senha, CPF, Endereco, Telefone)
    VALUES (vEmail, vNome, vSenha, vCPF, vEndereco, vTelefone);

    SET vIdCli = LAST_INSERT_ID();
END $$
DELIMITER ;
-- call da procedure de cadastro:
CALL sp_CadastroUsu(
    'arthur@gmail.com',
    'Arthur dos Santos Reimberg',
    'art123',
    '12345678901',
    'Rua Algum Lugar, Número 42',
    '11945302356',
    @vIdCli
);

CALL sp_CadastroUsu(
    'lucas@gmail.com',
    'Lucas Hora',
    'luc123',
    '12345678912',
    'Rua Algum Lugar, Número 40',
    '11945302359',
    @vIdCli
);


-- procedure adicionar nivel de acesso
DELIMITER $$
CREATE PROCEDURE sp_AdicionarNivel(
	vUsuId INT,
    vNivelId INT
)
BEGIN
	INSERT INTO tbUsuNivel(IdUsuario, IdNivel)
    VALUES(vUsuId, vNivelId);
END$$

DELIMITER ;
CALL sp_AdicionarNivel(1, 2)

-- Procedure Cadastrar Produto
-- drop procedure sp_CadastrarProduto
DELIMITER $$
CREATE PROCEDURE sp_CadastrarProduto(
	vNomeProduto VARCHAR(100),
    vPreco DECIMAL(8,2),
    vDescricao varchar(2500),
    vMarca VARCHAR(100),
    vAvaliacao DECIMAL(2,1),
    vCategoria VARCHAR(100),
    vQtdEstoque INT
)
BEGIN
	DECLARE vIdProduto INT;
    
    -- Salva os valores do produto
	INSERT INTO tbProduto(NomeProduto, Descricao, Preco, Marca, Categoria,Avaliacao, EstadoProduto)
    VALUES(vNomeProduto, vDescricao, vPreco, vMarca, vCategoria,vAvaliacao, true);
    SET vIdProduto = LAST_INSERT_ID();

    -- Salva a quantidade em estoque
    INSERT INTO tbEstoque(IdProduto, QtdEstoque, Disponibilidade)
    VALUES(vIdProduto, vQtdEstoque, true);
    
END $$
DELIMITER ;
-- Produto 1 
CALL sp_CadastrarProduto(
	'Bateria Exemplo', 
    2000.99,
    'Bateria vendida pela loja y, 
    com as especificaçoes a seguir: xxxxxxxxxxxxxxxxxxx, xxxxxxxxxxxxxxx, xxxxxxxxxxxx' ,
    'Marca Exemplo', 
    4.5,
    'Percursão',
    20
);
-- Produto 2 
CALL sp_CadastrarProduto(
	'Guitarra Exemplo', 
    1500.00,
    'Guitarra vendida pela loja y, 
    com as especificaçoes a seguir: xxxxxxxxxxxxxxxxxxx, xxxxxxxxxxxxxxx, xxxxxxxxxxxx' ,
    'Marca Exemplo', 
    4.5,
    'Cordas',
    20
);



DELIMITER $$
CREATE PROCEDURE sp_AdicionarImagens( 
	vIdProduto INT,
    vImagemUrl VARCHAR(255)
)
BEGIN
    INSERT INTO tbImagens(IdProduto,UrlImagem)
    VALUES(vIdProduto,vImagemUrl);
END $$
DELIMITER ;
CALL sp_AdicionarImagens(1,'www.imagem.url.com.br3');


DELIMITER $$
CREATE PROCEDURE sp_AdministrarCarrinho(
    IN vIdUsuario INT,
    IN vIdProduto INT,
    IN vQtd INT
)
BEGIN
    DECLARE vIdCarrinho INT DEFAULT NULL;
    DECLARE vPrecoUnidadeCar DECIMAL(8,2);
    DECLARE vSubTotal DECIMAL(8,2);
    DECLARE vIdItemCarrinho INT DEFAULT NULL;

    -- Verifica se o carrinho do usuário já existe (ativo)
    SELECT IdCarrinho 
    INTO vIdCarrinho 
    FROM tbCarrinho 
    WHERE IdUsuario = vIdUsuario;

    -- Se o carrinho ainda não existir, cria um novo
    IF vIdCarrinho IS NULL THEN
        SELECT Preco INTO vPrecoUnidadeCar 
        FROM tbProduto 
        WHERE IdProduto = vIdProduto;

        SET vSubTotal = vPrecoUnidadeCar * vQtd;

        INSERT INTO tbCarrinho (IdUsuario, DataCriacao, Estado, ValorTotal)
        VALUES (vIdUsuario, CURDATE(), 1, vSubTotal);

        SET vIdCarrinho = LAST_INSERT_ID();

        INSERT INTO tbItemCarrinho (IdCarrinho, IdProduto, QtdItemCar, PrecoUnidadeCar, SubTotal)
        VALUES (vIdCarrinho, vIdProduto, vQtd, vPrecoUnidadeCar, vSubTotal);

    ELSE
        -- Carrinho já existe, verificar se o produto já foi adicionado
        SELECT IdItemCarrinho 
        INTO vIdItemCarrinho 
        FROM tbItemCarrinho 
        WHERE IdCarrinho = vIdCarrinho AND IdProduto = vIdProduto;

        IF vIdItemCarrinho IS NULL THEN
            -- Produto ainda não está no carrinho
            SELECT Preco INTO vPrecoUnidadeCar 
            FROM tbProduto 
            WHERE IdProduto = vIdProduto;

            SET vSubTotal = vPrecoUnidadeCar * vQtd;

            INSERT INTO tbItemCarrinho (IdCarrinho, IdProduto, QtdItemCar, PrecoUnidadeCar, SubTotal)
            VALUES (vIdCarrinho, vIdProduto, vQtd, vPrecoUnidadeCar, vSubTotal);
            -- Atualiza o total do carrinho com base na soma dos subtotais
			UPDATE tbCarrinho
			SET ValorTotal = ValorTotal + vSubTotal
			WHERE IdCarrinho = vIdCarrinho;
        END IF;
    END IF;
END $$
DELIMITER ;

select * from tbcarrinho;
SELECT * FROM TBITEMCARRINHO;
CALL sp_AdministrarCarrinho(1, 2, 5);



DELIMITER $$
CREATE PROCEDURE sp_GerarVenda(
    IN vIdUsuario INT, 
    IN vTipoPag VARCHAR(50),
    IN vIdCarrinho INT
)
BEGIN
	DECLARE vIdVenda INT;
    DECLARE vIdProduto INT;
    DECLARE vQtdItem INT;
    DECLARE vPreco DECIMAL(8,2);
	DECLARE vQtdTotal INT;
    DECLARE vValorTotal DECIMAL(8,2);
    DECLARE done INT DEFAULT 0;
    
    -- Cursor que percorre todos os itens do carrinho
    DECLARE curItens CURSOR FOR
    SELECT IdProduto, QtdItemCar, PrecoUnidadeCar
    FROM tbItemCarrinho
    WHERE IdCarrinho = vIdCarrinho;
    
    DECLARE CONTINUE HANDLER FOR NOT FOUND SET done = 1; 
    
   -- Encontrar o valor total do carrinho
   SELECT ValorTotal INTO vValorTotal FROM tbCarrinho WHERE IdCarrinho = vIdCarrinho AND Estado = 1;
   -- Encontrar quantidade total
   SELECT SUM(QtdItemCar) INTO vQtdTotal from tbItemCarrinho WHERE IdCarrinho = vIdCarrinho;
   -- cria a venda 
   INSERT INTO tbVenda(IdUsuario, TipoPag, QtdTotal, ValorTotal)
   VALUES(vIdUsuario, vTipoPag, vQtdTotal, vValorTotal);
   SET vIdVenda = LAST_INSERT_ID();
   
   -- Abrir o cursor/loop
   OPEN curItens;
	read_loop: LOOP
			FETCH curItens INTO vIdProduto, vQtdItem, vPreco;
			IF done THEN
				LEAVE read_loop;
			END IF;
			
		-- Insere item de venda
		INSERT INTO tbItemVenda(IdVenda, IdProduto, PrecoUni, Qtd)
		VALUES(vIdVenda, vIdProduto, vPreco, vQtdItem);
		
		-- Atualizar estoque do produto
		UPDATE tbEstoque
		SET QtdEstoque = QtdEstoque - vQtdItem, Disponibilidade = IF (QtdEstoque - vQtdItem <= 0, 0, 1)
		WHERE IdProduto = vIdProduto;
	END LOOP;
	
    CLOSE curItens;
    
    UPDATE tbCarrinho
    SET Estado = 0
    WHERE IdCarrinho = vIdCarrinho;
    
END $$
DELIMITER ;
CALL sp_GerarVenda(1,'Pix',1);

select * from tbVenda;

-- Views

-- Buscar Produtos
create view vw_ExibirProdutos as
SELECT 
	p.IdProduto,
    p.NomeProduto,
	p.Descricao,
	p.Preco,
    p.Marca,
    p.Avaliacao,
    p.Categoria,
    ip.UrlImagem,
    e.Disponibilidade
FROM tbProduto AS p 
INNER JOIN tbImagens AS ip
	ON p.IdProduto = ip.IdProduto
INNER JOIN tbImagens AS i
	ON ip.IdImagem = i.IdImagem
INNER JOIN tbEstoque AS e
	ON p.IdProduto = e.IdProduto
WHERE e.Disponibilidade = TRUE;

SELECT * FROM vw_ExibirProdutos;

-- Exibição de Vendas
CREATE VIEW vw_VendaDetalhada AS
SELECT 
    v.IdVenda,
    v.IdUsuario,
    u.Nome AS NomeUsuario,
    v.TipoPag,
    iv.IdProduto,
    p.NomeProduto AS NomeProduto,
    iv.PrecoUni,
    iv.Qtd AS QtdItem,
    (iv.PrecoUni * iv.Qtd) AS Subtotal
FROM tbVenda AS v
INNER JOIN tbItemVenda AS iv ON v.IdVenda = iv.IdVenda
INNER JOIN tbProduto AS p ON iv.IdProduto = p.IdProduto
INNER JOIN tbUsuario AS u ON v.IdUsuario = u.IdUsuario
ORDER BY IdVenda DESC;

select * from vw_VendaDetalhada