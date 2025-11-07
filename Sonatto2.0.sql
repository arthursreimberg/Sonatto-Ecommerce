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
    Telefone VARCHAR(11) NOT NULL,
    Estado BIT NOT NULL DEFAULT 1
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
    IdProduto INT PRIMARY KEY AUTO_INCREMENT,
    NomeProduto VARCHAR(100) NOT NULL,
    Descricao VARCHAR(2500) NOT NULL,
    Preco DECIMAL(8,2) NOT NULL,
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

CREATE TABLE tbHistoricoAcao(
	IdHistorico INT PRIMARY KEY AUTO_INCREMENT,
    Acao VARCHAR(50) NOT NULL,
    DataAcao DATETIME NOT NULL,
    IdUsuario INT NOT NULL,
    FOREIGN KEY (IdUsuario) REFERENCES tbUsuario(IdUsuario)
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

DELIMITER $$
CREATE PROCEDURE sp_AlterarUsu(
	vIdUsuario INT,
    vEmail VARCHAR(50),
    vNome VARCHAR(100),
    vSenha VARCHAR(100),
    vCPF VARCHAR(11),
    vEndereco VARCHAR(150),
    vTelefone VARCHAR(11)
)
BEGIN
	UPDATE tbUsuario
		SET Email = vEmail,
			Nome = vNome,
            Senha = vSenha,
            CPF = vCPF,
            Endereco = vEndereco,
            Telefone = vTelefone
	WHERE IdUsuario = vIdUsuario;
END $$
DELIMITER ;
CALL sp_AlterarUsu(
	1,
    'alteracao@gmail.com',
    'Arthur Alteracao',
    'alt123',
    '12345678913',
    'Rua Algum Lugar, Número 40',
    '11945302359'
);

select * from tbUsuario


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
    vQtdEstoque INT,
    vIdUsuario INT
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
    
    -- Salva o histórico de inserção
    INSERT INTO tbHistoricoAcao(Acao, DataAcao, IdUsuario)
    VALUES('Adicionar Produto', CURRENT_TIMESTAMP(), vIdUsuario);
    
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
    20,
    1
);

DELIMITER $$
CREATE PROCEDURE sp_AlterarProduto(
	vIdProduto INT,
	vNomeProduto VARCHAR(100),
    vPreco DECIMAL(8,2),
    vDescricao varchar(2500),
    vMarca VARCHAR(100),
    vAvaliacao DECIMAL(2,1),
    vCategoria VARCHAR(100),
    vQtd INT,
    vIdUsuario INT,
    vAcao varchar(50)
)
BEGIN
	IF(vAcao = 'alterar') THEN
		-- Atualiza os valores do produto
		UPDATE tbProduto 
			SET NomeProduto = vNomeProduto, 
			Descricao =vDescricao, 
			Preco =vPreco, 
			Marca = vMarca, 
			Categoria = vCategoria,
			Avaliacao = vAvaliacao, 
			EstadoProduto = true
		WHERE IdProduto = vIdProduto;
        -- Atualiza o estoque
		UPDATE tbEstoque
			SET QtdEstoque = vQtd
		WHERE IdProduto = vIdProduto;
		-- Salva o histórico da alteração  
		INSERT INTO tbHistoricoAcao(Acao, DataAcao, IdUsuario)
		VALUES('Alterar Produto', CURRENT_TIMESTAMP(), vIdUsuario);
        
	   ELSE IF(vAcao = 'deletar') THEN
			UPDATE tbProduto
				SET EstadoProduto = false
            WHERE IdProduto = vIdProduto;
			-- Salva o histórico da exclusão/desativação do produto
			INSERT INTO tbHistoricoAcao(Acao, DataAcao, IdUsuario)
			VALUES('Deletar Produto', CURRENT_TIMESTAMP(), vIdUsuario);
		END IF;
    END IF;
END $$
DELIMITER ;


call sp_AlterarProduto(1,'alteracao exemplo','100.99','teste de alteração','alguma marca','5.0','cordas', 10, 1,'alterar')
call sp_AlterarProduto(1,'alteracao exemplo','100.99','teste de alteração','alguma marca','5.0','cordas', 10, 1,'alterar')
SELECT * FROM TBHISTORICOACAO

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

CALL sp_AdministrarCarrinho(1, 1, 5);
select * from tbcarrinho;
SELECT * FROM TBITEMCARRINHO;


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


DELIMITER $$

CREATE PROCEDURE sp_ExibirProduto(IN vIdProduto INT)
BEGIN
    SELECT *
    FROM vw_ExibirProdutos
    WHERE IdProduto = vIdProduto;
END $$

DELIMITER ;

call sp_ExibirProduto(1);
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

select * from vw_VendaDetalhada ;




CALL sp_CadastrarProduto( 'Piano Caziuk Clasic 02 Preto Brilho 88 Teclas', 33000.00, 'Os pianos cenográficos CAZIUK esta em alta entre os artistas, por ser leve, fácil de transportar, não desafina, produto durável, surpreende com a sua presença, dando um glamour onde se encontra, um item de decoração de luxo, seu brilho atraia a atenção de todos, é bem requisitado em festas de casamento, e outros eventos, seu valor ainda esta bem acessível, o investimento se retorna rapidamente com os alugueis, que hj esta na faixa de 2000,00 a 3000,00 á diária. nunca desvaloriza. fácil manutenção, para os pianistas, tecladista, decoradores de festas, pode se tornar mais uma fonte de renda fixa com seu aluguel, uma linda decoração para seu comercio, sala e Igrejas, seu valor de mercado apresenta alta, pela procura mantendo a valorização por ser um item único. Os pianos Cenográficos CAZIUK são fabricado por um Luthier profissional, que tem o extremo cuidado na fabricação de seus pianos. ADQUIRA LOGO O SEU PIANO CENOGRAFICO CAZIUK. Prazo para a fabricação de 120 A 150 dias, entrega depende da distancia da localidade . O piano de cauda cenográfico irá acompanhado de um sistema de amplificação no seu interior para fonte 12v , 110v, 220v , com ajuste de volume, grave, médio e agudos, contando com alto falante de 10 polegadas JBL 200watts rms, uma corneta e um twiter divisor de frequência passivo no interior do piano, como se fosse uma grande caixa de som ativa, de forma imperceptível, produzindo o som em seu próprio corpo. *Devido as curvas da cauda do piano cenográfico, o permite produzir um som muito mais aparente de um piano acústico real ,com reverberação, devido a este formato curvado, do que caixas quadradas ou até mesmo cubos. fazendo que os timbres de seu instrumento fique muito mais parecido com os originais acústicos. Cor; Black Piano *Borda da tampa moldurada. os pés contem negativos. * possui tampa com regulagem sobre as teclas. * Banqueta de brinde ate 28 de julho de 2025 Os pés do piano contem rodinhas com travastes, seus pés e suporte de pedal são removíveis, pois são somente encaixados, sua retirada facilitar o transporte. Regulagem internas de altura ajustável para pianos de: 11cm a 17cm, comprimento do encaixe do piano de 1,28 a 1,445 , largura ajustável de 21cm a 26cm, possibilitando o uso de vários modelos de piano digitais. Medidas de altura da caixa externa 30cm a 37cm. Medidas dos pés 63cm a 70 cm. Comprimento 1,60cm Largura 1,50cm.', 'CAZIUK', 4.5, 'Teclas', 20, 1 );
CALL sp_AdicionarImagens(1,'https://http2.mlstatic.com/D_NQ_NP_2X_927664-MLB89452613385_082025-F.webp');
CALL sp_AdicionarImagens(1,'https://http2.mlstatic.com/D_Q_NP_2X_712318-MLB76839082850_062024-R.webp');
CALL sp_AdicionarImagens(1,'https://http2.mlstatic.com/D_Q_NP_2X_923933-MLB82554997263_022025-R.webp');



CALL sp_CadastrarProduto( 'Orgão Eletrônico 44 + 44 Teclas Tokai Md-10 Evo Cor Marrom-escuro', 10359.00, '- 44 teclas fechadas - 13 notas na pedaleira - Presets 160 sons - Drawbars completo para ambos teclados - Dual voice para ambos teclados e pedaleira, possibilitando utilizar 2 Presets simultaneamente. - Mudança de registração no pedal de volume (Igual Yamaha) - Controle de intensidade: Vibrato, chorus e sustain - Metrônomo simples e composto (com vozes Ex 4 por 4: 1, 2, 3, 4) - Pedaleira maior para melhor execução - QR code no visor para manual online. - Novo sistema acústico - Banqueta personalizada MD-10 Evo Wengue (Marrom)', 'Tokai', 4.5, 'Teclas', 20, 1 );
CALL sp_CadastrarProduto( 'Acordeon 80 Baixos Michael Acm8007n Spb C\ Bag Rodinhas Cor Preto sólido', 7721.00, 'ACORDEON MICHAEL ACM8007N SPB 80 BAIXOS O Michael ACM8007N é um acordeon de 80 baixos, confortável e com timbre intenso. Possui 7 registros para a mão direita e 2 para a esquerda, além de 37 teclas macias. É construído com madeira de lei, uma matéria-prima robusta, e conta com componentes de qualidade, como as palhetas de aço inoxidável e o fole de linho e couro, protegido por cantoneiras de metal. Acompanha case e alça para facilitar o transporte. ESPECIFICAÇÕES TÉCNICAS: • Acordeon 80 baixos • Palheta em aço inoxidável • 37 teclas • 7 registros de mão direita e 2 registros de mão esquerda • Terça de Voz • Estrutura de madeira nobre • Acabamento refinado – Alto Brilho • Fole com revestimento em linho e couro • Cantoneiras externas do fole em metal • Acompanha case com rodinhas, alça retrátil, bolso externo e alças para acordeon', 'Micheal', 4.5, 'Teclas', 20, 1 );
CALL sp_CadastrarProduto('Harmonium Harmonio Indiano Sarat Sardar And Sons 1950', 5400.00, 'peça maravilhosa, dos anos 50 è uma peça herdada, nao sei tocar, nao sou musicista, nao sei sobre afinação. porem enchendo o fole, está acionando todas as teclas, e botoes madeira integra, minimas sinais do tempo e do uso, porem sem danos lindissimo', 'Calcutá', 4.5, 'Teclas', 20, 1 );
CALL sp_CadastrarProduto( 'Teclado Arranjador Casio Mz-x500 Com 61 Teclas Sensitivas Cor Azul', 4823.90, 'A Casio é uma empresa com uma longa trajetória no mercado que se destaca por oferecer produtos originais e inovadores. Sem dúvida, seus teclados musicais são uma excelente opção. Vá para o próximo nível Com suas 61 teclas, você poderá tocar uma grande variedade de obras e mergulhar no mundo do intérprete musical. Ideal para níveis intermediários que querem se superar e seguir o caminho dos grandes músicos. Interpretações que transportam As teclas sensíveis ao toque capturam a força e a velocidade com que são pressionadas e permitem ajustar a intensidade do som para obter maior expressividade ao tocar. Prepare-se para brilhar! Mais ritmos, mais música Desfrute de um grande banco de estilos e ritmos musicais. Deixe-se guiar e atreva-se a improvisar nas pistas. Sonoridades múltiplas A grande variedade de timbres que ele oferece permitirá que você escolha entre diferentes instrumentos ao compor e enriquecer sua interpretação. Valorize suas melhores interpretações Graças ao seu gravador, você poderá ouvir suas execuções repetidas vezes, aperfeiçoar sua digitação e salvar suas versões favoritas. Construa seu estúdio de produção Com o controlador MIDI, você poderá conectar seu instrumento ao seu computador e a outros dispositivos para retratar suas composições, ajustar parâmetros e criar obras exclusivas. Afinação garantida Possui controle de afinação, função que o permite afinar quando necessário para que seu instrumento soe sempre perfeito.', 'Casio', 4.5, 'Teclas', 20, 1 );
CALL sp_CadastrarProduto( 'Rhodes Suitcase Mark I Eighty Eight Piano Elétrico Vintage', 50000.00, 'Piano Fender Vintage funcionando perfeitamente sem quaisquer problemas.', 'Rhodes', 4.5, 'Teclas', 20, 1 );
CALL sp_CadastrarProduto( 'Escaleta De Sopro 32 Teclas C/ Capa Melodica Infantil Preta Bertô Mangueira e Bocal', 109.85, 'Escaleta Melodica Bertô Preta 32 Teclas + Capa Brinde Instrumento de sopro...', 'Bertô', 4.5, 'Teclas', 20, 1 );
CALL sp_CadastrarProduto( 'Teclado Musical Profissional Piano Eletronico 61 Teclas Cor Preto', 379.00, 'Melhore suas habilidades...', 'SmartVox', 4.5, 'Teclas', 20, 1 );
CALL sp_CadastrarProduto( 'Teclado Musical (piano Elétrico) Com 88 Teclas Responsivas', 1677.77, 'Este teclado digital...', 'Waver', 4.5, 'Teclas', 20, 1 );
CALL sp_CadastrarProduto( 'Orgão Eletronico Acordes Classic', 5190.00, 'O Órgão Eletrônico Acordes Classic...', 'ACORDES', 4.5, 'Teclas', 20, 1 );
CALL sp_CadastrarProduto( 'Timpano Magnum Em Fibra De Vidro 26 Com Estojo', 15225.00, 'Sistema pedal para bloco...', 'Magnum', 4.5, 'Percussão', 20, 1 );
CALL sp_CadastrarProduto( 'Bateria Eletrônica com Peles esh DM-110 - Nux', 3255.88, 'A NUX DM-110 é um kit...', 'Nux', 4.5, 'Percussão', 20, 1 );
CALL sp_CadastrarProduto( 'Pad de tom PDA120L-BK', 1483.59, 'Construído em madeira premium...', 'Roland', 4.5, 'Percussão', 20, 1 );
CALL sp_CadastrarProduto( 'Tamborim 6 Frisadp Reels Lilás 595-ALRL - Gope', 137.08, 'Corpo: Alumínio...', 'GOPE', 4.5, 'Percussão', 20, 1 );
CALL sp_CadastrarProduto( 'Reco Reco ALuminio G2 Lilás 766-L - Gope', 219.88, 'O Reco Reco G2...', 'GOPE', 4.5, 'Percussão', 20, 1 );
CALL sp_CadastrarProduto( 'Ganza Twist Medium LP-441T-M - LP', 219.88, 'A série LP Twist...', 'Twist', 4.5, 'Percussão', 20, 1 );
CALL sp_CadastrarProduto( 'Stagg BW-200-BK Bongôs latinos de madeira', 703.80, '6,5 polegadas e 7,5 polegadas...', 'Stagg', 4.5, 'Percussão', 20, 1 );
CALL sp_CadastrarProduto( 'Cowbell Pearl 4 PCB4', 154.80, 'Tamanho pequeno 4"...', 'Pearl', 4.5, 'Percussão', 20, 1 );
CALL sp_CadastrarProduto( 'Atabaque c Borda 1m de altura', 898.99, '- Feito em madeira de reflorestamento...', 'Jair', 4.5, 'Percussão', 20, 1 );
CALL sp_CadastrarProduto( 'Agogo Contmeporanea Duplo cromado 01c', 212.35, 'O Agogo Duplo...', 'Contemporanea', 4.5, 'Percussão', 20, 1 );
CALL sp_CadastrarProduto( 'Gaita De Boca Hohner Harmônica Golden Melody G', 497.90, 'A Hohner Golden Melody...', 'Hohner', 4.5, 'Sopro', 20, 1 );
CALL sp_CadastrarProduto( 'Flauta Transversal Spring C Niquelada 16 Chaves Profissional', 1550.99, 'Flauta Transversal Spring C...', 'Panorama', 4.5, 'Sopro', 20, 1 );
CALL sp_CadastrarProduto( 'Eagle Clarinete 17 Chaves Sib Cl04n Cor Preto Cor das chaves Niqueladas', 1299.90, 'O Clarinete CL 04...', 'Eagle', 4.5, 'Sopro', 20, 1 );
CALL sp_CadastrarProduto( 'Saxofone Alto Jupiter Jas 767 Laqueado Dourado', 5990.00, '- Afinação em Eb...', 'Jupiter', 4.5, 'Sopro', 20, 1 );


CALL sp_AdicionarImagens(2,'https://http2.mlstatic.com/D_NQ_NP_2X_907425-MLU77761939047_072024-F.webp');
CALL sp_AdicionarImagens(2,'https://http2.mlstatic.com/D_Q_NP_2X_783167-MLU77544050746_072024-R.webp');
CALL sp_AdicionarImagens(2,'https://http2.mlstatic.com/D_Q_NP_2X_676579-MLU77761939123_072024-R.webp');

CALL sp_AdicionarImagens(3,'https://http2.mlstatic.com/D_NQ_NP_2X_670924-MLA94167840158_102025-F.webp');
CALL sp_AdicionarImagens(3,'https://http2.mlstatic.com/D_Q_NP_2X_884625-MLA94167622038_102025-R.webp');
CALL sp_AdicionarImagens(3,'https://http2.mlstatic.com/D_Q_NP_2X_865741-MLA94167512582_102025-R.webp');

CALL sp_AdicionarImagens(4,'https://http2.mlstatic.com/D_Q_NP_2X_874595-MLB89491744919_082025-R.webp');
CALL sp_AdicionarImagens(4,'https://http2.mlstatic.com/D_Q_NP_2X_748001-MLB82065322059_012025-R.webp');
CALL sp_AdicionarImagens(4,'https://http2.mlstatic.com/D_Q_NP_2X_618741-MLB81786606918_012025-R.webp');

CALL sp_AdicionarImagens(5,'https://http2.mlstatic.com/D_NQ_NP_2X_889605-MLA95359295085_102025-F.webp');
CALL sp_AdicionarImagens(5,'https://http2.mlstatic.com/D_Q_NP_2X_973100-MLA94466256207_102025-R.webp');
CALL sp_AdicionarImagens(5,'https://http2.mlstatic.com/D_Q_NP_2X_636209-MLA94466295941_102025-R.webp');

CALL sp_AdicionarImagens(6,'https://http2.mlstatic.com/D_Q_NP_2X_903280-MLB84246085393_042025-R-rhodes-suitcase-mark-i-eighty-eight-piano-eletrico-vintage.webp');
CALL sp_AdicionarImagens(6,'https://http2.mlstatic.com/D_Q_NP_2X_724302-MLB83951019030_042025-R-rhodes-suitcase-mark-i-eighty-eight-piano-eletrico-vintage.webp');
CALL sp_AdicionarImagens(6,'https://http2.mlstatic.com/D_Q_NP_2X_623769-MLB84245832087_042025-R-rhodes-suitcase-mark-i-eighty-eight-piano-eletrico-vintage.webp');

CALL sp_AdicionarImagens(7,'https://http2.mlstatic.com/D_Q_NP_2X_735339-MLA94937923400_102025-R.webp');
CALL sp_AdicionarImagens(7,'https://http2.mlstatic.com/D_Q_NP_2X_794357-MLA84313823302_052025-R.webp');
CALL sp_AdicionarImagens(7,'https://http2.mlstatic.com/D_Q_NP_2X_786678-MLA84611103569_052025-R.webp');

CALL sp_AdicionarImagens(8,'https://http2.mlstatic.com/D_NQ_NP_2X_608318-MLA94949295910_102025-F.webp');
CALL sp_AdicionarImagens(8,'https://http2.mlstatic.com/D_Q_NP_2X_993137-MLA94467413363_102025-R.webp');
CALL sp_AdicionarImagens(8,'https://http2.mlstatic.com/D_Q_NP_2X_886407-MLA94018522398_102025-R.webp');

CALL sp_AdicionarImagens(9,'https://http2.mlstatic.com/D_Q_NP_2X_801267-MLB90360292991_082025-R-teclado-musical-piano-eletrico-com-88-teclas-responsivas.webp');
CALL sp_AdicionarImagens(9,'https://http2.mlstatic.com/D_Q_NP_2X_714981-MLB83366616151_032025-R-teclado-musical-piano-eletrico-com-88-teclas-responsivas.webp');
CALL sp_AdicionarImagens(9,'https://http2.mlstatic.com/D_Q_NP_2X_610167-MLB84983346673_052025-R-teclado-musical-piano-eletrico-com-88-teclas-responsivas.webp');

CALL sp_AdicionarImagens(10,'https://http2.mlstatic.com/D_NQ_NP_2X_957015-MLB89576569286_082025-F-orgo-eletronico-acordes-classic-loja-jubi.webp');
CALL sp_AdicionarImagens(10,'https://http2.mlstatic.com/D_Q_NP_2X_700892-MLB89576529710_082025-R-orgo-eletronico-acordes-classic-loja-jubi.webp');
CALL sp_AdicionarImagens(10,'https://http2.mlstatic.com/D_Q_NP_2X_725920-MLB81326194060_122024-R-orgo-eletronico-acordes-classic-loja-jubi.webp');

CALL sp_AdicionarImagens(11,'https://http2.mlstatic.com/D_Q_NP_836813-MLB69691680173_052023-R.webp');
CALL sp_AdicionarImagens(11,'https://http2.mlstatic.com/D_Q_NP_816053-MLB69691800405_052023-R.webp');
CALL sp_AdicionarImagens(11,'https://http2.mlstatic.com/D_Q_NP_966233-MLB69692017527_052023-R.webp');

CALL sp_AdicionarImagens(12,'https://ninjasom.vtexassets.com/arquivos/ids/208918-1600-auto');
CALL sp_AdicionarImagens(12,'https://ninjasom.vtexassets.com/arquivos/ids/208919-1600-auto');
CALL sp_AdicionarImagens(12,'https://ninjasom.vtexassets.com/arquivos/ids/208920-1600-auto');

CALL sp_AdicionarImagens(13,'https://ninjasom.vtexassets.com/arquivos/ids/180789-1600-auto');
CALL sp_AdicionarImagens(13,'https://ninjasom.vtexassets.com/arquivos/ids/180787-1600-auto');
CALL sp_AdicionarImagens(13,'https://ninjasom.vtexassets.com/arquivos/ids/180788-1600-auto');

CALL sp_AdicionarImagens(14,'https://ninjasom.vtexassets.com/arquivos/ids/204048-1600-auto');
CALL sp_AdicionarImagens(14,'https://ninjasom.vtexassets.com/arquivos/ids/204049-1600-auto');
CALL sp_AdicionarImagens(14,'https://ninjasom.vtexassets.com/arquivos/ids/204050-1600-auto');

CALL sp_AdicionarImagens(15,'https://ninjasom.vtexassets.com/arquivos/ids/206888-1600-auto');
CALL sp_AdicionarImagens(15,'https://ninjasom.vtexassets.com/arquivos/ids/206889-1600-auto');
CALL sp_AdicionarImagens(15,'https://ninjasom.vtexassets.com/arquivos/ids/206890-1600-auto');

CALL sp_AdicionarImagens(16,'https://ninjasom.vtexassets.com/arquivos/ids/209734-1600-auto');
CALL sp_AdicionarImagens(16,'https://ninjasom.vtexassets.com/arquivos/ids/209735-1600-auto');
CALL sp_AdicionarImagens(16,'https://ninjasom.vtexassets.com/arquivos/ids/209736-1600-auto');

CALL sp_AdicionarImagens(17,'https://m.media-amazon.com/images/I/51cmNOSdHfL._AC_SL500_.jpg');
CALL sp_AdicionarImagens(17,'https://m.media-amazon.com/images/I/51x988mdL0L._AC_SY450_.jpg');

CALL sp_AdicionarImagens(18,'https://d58a5eovtl12n.cloudfront.net/Custom/Content/Products/68/46/68466_cowbell-pearl-4-primero-pcb4_z2_637836415655836169.webp');
CALL sp_AdicionarImagens(18,'https://d58a5eovtl12n.cloudfront.net/Custom/Content/Products/68/46/68466_cowbell-pearl-4-primero-pcb4_z1_637836415644898057.webp');
CALL sp_AdicionarImagens(18,'https://d58a5eovtl12n.cloudfront.net/Custom/Content/Products/68/46/68466_cowbell-pearl-4-primero-pcb4_z1_637836415643492407.webp');

CALL sp_AdicionarImagens(19,'https://http2.mlstatic.com/D_Q_NP_726809-MLB48073627699_102021-R.webp');
CALL sp_AdicionarImagens(19,'https://http2.mlstatic.com/D_Q_NP_866984-MLB48073674062_102021-R.webp');
CALL sp_AdicionarImagens(19,'https://http2.mlstatic.com/D_Q_NP_915024-MLB48073630680_102021-R.webp');

CALL sp_AdicionarImagens(20,'https://http2.mlstatic.com/D_Q_NP_928620-MLU72714160740_112023-R.webp');
CALL sp_AdicionarImagens(20,'https://http2.mlstatic.com/D_Q_NP_804233-MLU72714091986_112023-R.webp');
CALL sp_AdicionarImagens(20,'https://http2.mlstatic.com/D_Q_NP_934993-MLA82624902145_022025-R.webp');

CALL sp_AdicionarImagens(21,'https://http2.mlstatic.com/D_Q_NP_638684-MLU78204712751_082024-R.webp');
CALL sp_AdicionarImagens(21,'https://http2.mlstatic.com/D_Q_NP_662607-MLU78204859895_082024-R.webp');
CALL sp_AdicionarImagens(21,'https://http2.mlstatic.com/D_Q_NP_709056-MLU78205285391_082024-R.webp');

CALL sp_AdicionarImagens(22,'https://http2.mlstatic.com/D_Q_NP_790298-MLB80619478167_112024-R.webp');
CALL sp_AdicionarImagens(22,'https://http2.mlstatic.com/D_Q_NP_628059-MLB73779294690_012024-R.webp');
CALL sp_AdicionarImagens(22,'https://http2.mlstatic.com/D_Q_NP_802581-MLB73779294694_012024-R.webp');

CALL sp_AdicionarImagens(23,'https://http2.mlstatic.com/D_Q_NP_691966-MLU75765493319_042024-R.webp');
CALL sp_AdicionarImagens(23,'https://http2.mlstatic.com/D_Q_NP_967520-MLA94968660386_102025-R.webp');
CALL sp_AdicionarImagens(23,'https://http2.mlstatic.com/D_Q_NP_890734-MLU79281599091_092024-R.webp');

CALL sp_AdicionarImagens(24,'https://http2.mlstatic.com/D_Q_NP_983239-MLA94925775190_102025-R.webp');
CALL sp_AdicionarImagens(24,'https://http2.mlstatic.com/D_Q_NP_757028-MLA93650312350_102025-R.webp');
CALL sp_AdicionarImagens(24,'https://http2.mlstatic.com/D_Q_NP_870043-MLA94072632529_102025-R.webp');

CALL sp_AdicionarImagens(25,'https://http2.mlstatic.com/D_Q_NP_911934-MLB89440611449_082025-R.webp');
CALL sp_AdicionarImagens(25,'https://http2.mlstatic.com/D_Q_NP_886515-MLB82517856317_022025-R.webp');
CALL sp_AdicionarImagens(25,'https://http2.mlstatic.com/D_Q_NP_837142-MLB82517640137_022025-R.webp');

CALL sp_AdicionarImagens(26,'https://http2.mlstatic.com/D_Q_NP_809257-MLU74139477731_012024-R.webp');
CALL sp_AdicionarImagens(26,'https://http2.mlstatic.com/D_Q_NP_778249-MLU73331836810_122023-R.webp');
CALL sp_AdicionarImagens(26,'https://http2.mlstatic.com/D_Q_NP_634458-MLA94918101970_102025-R.webp');

CALL sp_AdicionarImagens(27,'https://http2.mlstatic.com/D_Q_NP_826628-MLB80504130582_112024-R.webp');
CALL sp_AdicionarImagens(27,'https://http2.mlstatic.com/D_Q_NP_831047-MLB80504120612_112024-R.webp');
CALL sp_AdicionarImagens(27,'https://http2.mlstatic.com/D_Q_NP_658258-MLB80504051676_112024-R.webp');

CALL sp_AdicionarImagens(28,'https://http2.mlstatic.com/D_Q_NP_631201-MLB90741557973_082025-R.webp');
CALL sp_AdicionarImagens(28,'https://http2.mlstatic.com/D_Q_NP_911350-MLB31703559840_082019-R.webp');
CALL sp_AdicionarImagens(28,'https://http2.mlstatic.com/D_Q_NP_969909-MLB31703558075_082019-R.webp');

CALL sp_AdicionarImagens(29,'https://http2.mlstatic.com/D_Q_NP_879269-MLA83488059841_042025-R.webp');
CALL sp_AdicionarImagens(29,'https://http2.mlstatic.com/D_Q_NP_782661-MLA95373392807_102025-R.webp');
CALL sp_AdicionarImagens(29,'https://http2.mlstatic.com/D_Q_NP_968974-MLA83196999770_042025-R.webp');

CALL sp_AdicionarImagens(30,'https://http2.mlstatic.com/D_Q_NP_683209-MLB89419596945_082025-R-16-furos-flauta-transversal-prateada-do-para-iniciantes.webp');
CALL sp_AdicionarImagens(30,'https://http2.mlstatic.com/D_Q_NP_703483-MLB88018377463_072025-R-16-furos-flauta-transversal-prateada-do-para-iniciantes.webp');
CALL sp_AdicionarImagens(30,'https://http2.mlstatic.com/D_Q_NP_637148-MLB88018229901_072025-R-16-furos-flauta-transversal-prateada-do-para-iniciantes.webp');

CALL sp_AdicionarImagens(31,'https://http2.mlstatic.com/D_NQ_NP_2X_933226-MLB80884628777_112024-F.webp');

CALL sp_AdicionarImagens(32,'https://static.mundomax.com.br/produtos/83260/550/1.webp');

CALL sp_AdicionarImagens(33,'https://m.media-amazon.com/images/I/61x-Acspv6L._AC_SY879_.jpg');

CALL sp_AdicionarImagens(34,'https://ae-pic-a1.aliexpress-media.com/kf/S5af225076288495ab6201d93c7512e23v.jpg_220x220q75.jpg_.avif');

CALL sp_AdicionarImagens(35,'https://http2.mlstatic.com/D_NQ_NP_2X_714421-MLB92348137747_092025-F.webp');

CALL sp_AdicionarImagens(36,'https://http2.mlstatic.com/D_NQ_NP_2X_876093-MLB90261559634_082025-F.webp');

CALL sp_AdicionarImagens(37,'https://www.teclacenter.com.br/media/catalog/product/cache/ddd3862c9b8cedec15fe8b38c4ddb4ee/t/c/tc_pearl_river_bpj60-n_-_03.webp');

CALL sp_AdicionarImagens(38,'https://m.media-amazon.com/images/I/71JlgP1p-FL._AC_SX679_.jpg');

CALL sp_AdicionarImagens(39,'https://lojaprobaixo.bwimg.com.br/lojaprobaixo/produtos/violoncelo-cecilio-cco-100-com-bag--1679076946.1479.jpg');

CALL sp_AdicionarImagens(40,'https://http2.mlstatic.com/D_NQ_NP_2X_933864-MLU74996459517_032024-F.webp');

