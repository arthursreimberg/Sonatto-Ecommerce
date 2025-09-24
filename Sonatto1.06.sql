drop database dbSonatto;
create database dbSonatto;
use dbSonatto;

show tables;

create table tbFuncionario(
IdFuncionario int primary key not null auto_increment,
Nome varchar(100) not null,
Cargo varchar(50) not null,
Email varchar(50)
);

create table tbLogin(
IdLogin int primary key not null auto_increment,
IdFuncionario int not null,
IdCliente int not null
-- Email varchar(50) not null unique,
-- Senha varchar(100) not null
);
-- 1 alter table tbLogin add constraint fk_idFuncionario_login foreign key(IdLogin) references tbFuncionario(IdFuncionario);
-- alter table tbLogin add constraint fk_idCliente_Login foreign key(Email) references tbCliente(Email);
-- 10 alter table tbLogin add constraint fk_idCliente_Login foreign key(IdCliente) references tbCliente(IdCliente);

create table tbCliente (
IdCliente int primary key not null auto_increment,
Email varchar(50) not null,
Nome varchar(100) not null,
CPF varchar(11) not null unique,
Endereco varchar(150) not null,
Telefone varchar(11) not null
);

create table tbCarrinho (
IdCarrinho int primary key not null auto_increment,
IdCliente int not null ,
DataCriacao datetime not null,
ItemCompra int not null
);
-- 2 alter table tbCarrinho add constraint fk_idCarrinho_IdCliente foreign key(IdCliente) references tbCliente(Email);


create table tbVenda(
IdVenda int primary key not null auto_increment,
IdProduto int not null,
IdCliente int not null,
TipoPagamento varchar(50) not null,
Quantidade int not null check(Quantidade > 0),
ValorTotal int not null
);
-- 3 alter table tbPagamento add constraint fk_idCompra_IdCliente foreign key(Email) references tbCliente(Email);


create table tbProduto(
IdProduto int primary key not null auto_increment,
NomeProduto varchar(100) not null,
Preco decimal(8,2) not null check(Preco>0),
ImagemURL varchar(255) not null, 
IdFornecedor int not null
);
-- 4 alter table tbProduto add constraint fk_idProduto_IdFornecedor foreign key(IdProduto) references tbFornecedor(IdFornecedor);


create table tbEstoque(
IdEstoque int primary key not null auto_increment,
IdProduto int not null, 
QuantidadeTotal int not null check(QuantidadeTotal>=0)
);
-- 5 alter table tbEstoque add constraint fk_IdEstoque_IdProduto foreign key(IdProduto) references tbProduto(IdProduto);


create table tbItemCompra(
IdVenda int not null,
IdProduto int not null,
Quantidade int not null check(Quantidade>0)
); 
-- 6 alter table tbItemCompra add constraint primary key(IdCompra, IdProduto);
-- 7 alter table tbItemCompra add constraint fk_IdItemCompra_IdCompra foreign key(IdCompra) references tbPagamento(IdCompra);
-- 8 alter table tbItemCompra add constraint fk_IdItemCompra_IdProduto foreign key(IdProduto) references tbProduto(IdProduto); 

create table tbNotaFiscal(
NumNotaFiscal int primary key not null auto_increment,
IdVenda int not null,
DataEmissao datetime not null,
Numero varchar(20) not null unique,
PrecoTotal int not null
);
-- 9 alter table tbNotaFiscal add constraint fk_IdNotaFiscal_IdCompra foreign key(IdCompra) references tbPagamento(IdCompra);

create table tbFornecedor(
Idfornecedor int primary key not null auto_increment,
Nome varchar(100) not null,
Contato varchar(50)
);

alter table tbLogin add constraint fk_idFuncionario_login foreign key(IdLogin) references tbFuncionario(IdFuncionario);
alter table tbCarrinho add constraint fk_idCarrinho_IdCliente foreign key(IdCliente) references tbCliente(IdCliente);
alter table tbVenda add constraint fk_idCompra_IdCliente foreign key(IdCliente) references tbCliente(IdCliente);
alter table tbProduto add constraint fk_idProduto_IdFornecedor foreign key(IdProduto) references tbFornecedor(IdFornecedor);
alter table tbEstoque add constraint fk_IdEstoque_IdProduto foreign key(IdProduto) references tbProduto(IdProduto);
alter table tbItemCompra add constraint primary key(IdVenda, IdProduto);
alter table tbItemCompra add constraint fk_IdItemCompra_IdVenda foreign key(IdVenda) references tbVenda(IdVenda);
alter table tbItemCompra add constraint fk_IdItemCompra_IdProduto foreign key(IdProduto) references tbProduto(IdProduto); 
alter table tbNotaFiscal add constraint fk_IdNotaFiscal_IdCompra foreign key(IdVenda) references tbVenda(IdVenda);
alter table tbLogin add constraint fk_idCliente_Login foreign key(IdCliente) references tbCliente(IdCliente);


