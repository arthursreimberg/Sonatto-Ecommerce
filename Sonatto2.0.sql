-- Apaga e recria o banco
DROP DATABASE dbSonatto;
CREATE DATABASE dbSonatto;
USE dbSonatto;

-- Tabela de Funcionário
CREATE TABLE tbFuncionario(
    IdFuncionario INT PRIMARY KEY AUTO_INCREMENT,
    Nome VARCHAR(100) NOT NULL,
    Senha VARCHAR(100) NOT NULL,
    Codigo VARCHAR(7) NOT NULL,
    Cargo VARCHAR(50) NOT NULL,
    Email VARCHAR(50)
);

-- Tabela de Cliente
CREATE TABLE tbCliente (
    IdCliente INT PRIMARY KEY AUTO_INCREMENT,
    Email VARCHAR(50) NOT NULL,
    Nome VARCHAR(100) NOT NULL,
    Senha VARCHAR(100) NOT NULL,
    CPF VARCHAR(11) NOT NULL UNIQUE,
    Endereco VARCHAR(150) NOT NULL,
    Telefone VARCHAR(11) NOT NULL
);

-- Tabela de Produto
CREATE TABLE tbProduto(
    IdProduto INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
    NomeProduto VARCHAR(100) NOT NULL,
    Preco DECIMAL(8,2) NOT NULL CHECK(Preco > 0),
    ImagemURL VARCHAR(255) NOT NULL,
	Marca VARCHAR(100) NOT NULL
);

-- Tabela de Imagens dos Produtos
CREATE TABLE tbImagensProduto(
	IdImagem int auto_increment primary key,
    IdProduto int not null ,
    UrlImagem varchar(255),
	CONSTRAINT fk_IdProduto_Imagens FOREIGN KEY(IdProduto) REFERENCES tbProduto(IdProduto)
);

-- Tabela de Estoque
CREATE TABLE tbEstoque(
    IdEstoque INT PRIMARY KEY AUTO_INCREMENT,
    IdProduto INT NOT NULL,
    QuantidadeTotal INT NOT NULL CHECK(QuantidadeTotal >= 0),
    Disponibilidade BIT NOT NULL,
    CONSTRAINT fk_IdEstoque_IdProduto FOREIGN KEY(IdProduto) REFERENCES tbProduto(IdProduto)
);


-- Tabela de Venda
CREATE TABLE tbVenda(
    IdVenda INT PRIMARY KEY AUTO_INCREMENT,
    IdProduto INT NOT NULL,
    IdCliente INT NOT NULL,
    TipoPagamento VARCHAR(50) NOT NULL,
    Quantidade INT NOT NULL CHECK(Quantidade > 0),
    ValorTotal INT NOT NULL,
    CONSTRAINT fk_idCompra_IdCliente FOREIGN KEY(IdCliente) REFERENCES tbCliente(IdCliente)
);

-- Tabela de ItemCompra
CREATE TABLE tbItemCompra(
    IdVenda INT NOT NULL,
    IdProduto INT NOT NULL,
    Quantidade INT NOT NULL CHECK(Quantidade > 0),
    PRIMARY KEY(IdVenda, IdProduto),
    CONSTRAINT fk_IdItemCompra_IdVenda FOREIGN KEY(IdVenda) REFERENCES tbVenda(IdVenda),
    CONSTRAINT fk_IdItemCompra_IdProduto FOREIGN KEY(IdProduto) REFERENCES tbProduto(IdProduto)
);

-- Tabela de Nota Fiscal
CREATE TABLE tbNotaFiscal(
    NumNotaFiscal INT PRIMARY KEY NOT NULL AUTO_INCREMENT,
    IdVenda INT NOT NULL,
    DataEmissao DATETIME NOT NULL,
    Numero VARCHAR(20) NOT NULL UNIQUE,
    PrecoTotal INT NOT NULL,
    CONSTRAINT fk_IdNotaFiscal_IdCompra FOREIGN KEY(IdVenda) REFERENCES tbVenda(IdVenda)
);

DELIMITER $$
CREATE PROCEDURE spAdicionarProdutoComImagens(
    IN pNomeProduto VARCHAR(100),
    IN pPreco DECIMAL(8,2),
    IN pImagemURL VARCHAR(255),
    IN pMarca VARCHAR(100),
    IN pJsonImagens JSON
)
BEGIN
    DECLARE vIdProduto INT;
    DECLARE vQtdImagens INT;
    DECLARE vIndex INT DEFAULT 0;
    DECLARE vUrlImagem VARCHAR(255);

    -- Inserir o produto principal
    INSERT INTO tbProduto (NomeProduto, Preco, ImagemURL, Marca)
    VALUES (pNomeProduto, pPreco, pImagemURL, pMarca);

    -- Pegar o ID do produto recém inserido
    SET vIdProduto = LAST_INSERT_ID();

    -- Quantidade de imagens no JSON
    SET vQtdImagens = JSON_LENGTH(pJsonImagens);

    -- Inserir todas as imagens adicionais (se houver)
    WHILE vIndex < vQtdImagens DO
        SET vUrlImagem = JSON_UNQUOTE(JSON_EXTRACT(pJsonImagens, CONCAT('$[', vIndex, ']')));

        INSERT INTO tbImagensProduto (IdProduto, UrlImagem)
        VALUES (vIdProduto, vUrlImagem);

        SET vIndex = vIndex + 1;
    END WHILE;
END$$
/*
Exemplo de call:
CALL spAdicionarProdutoComImagens(
    'Mouse Gamer RGB',
    149.99,
    'https://meusite.com/produtos/mouse_principal.jpg',
    'Logitech',
    '["https://meusite.com/produtos/mouse1.jpg",
      "https://meusite.com/produtos/mouse2.jpg",
      "https://meusite.com/produtos/mouse3.jpg"]'
);
*/
DELIMITER ;

CALL spAdicionarProdutoComImagens(
    'Teste',
    149.99,
    'https://meusite.com/produtos/mouse_principal.jpg',
    'Logitech',
    '["https://meusite.com/produtos/mouse1.jpg",
      "https://meusite.com/produtos/mouse2.jpg",
      "https://meusite.com/produtos/mouse3.jpg"]'
);
SELECT * FROM tbProduto;
SELECT * FROM tbImagensProduto;