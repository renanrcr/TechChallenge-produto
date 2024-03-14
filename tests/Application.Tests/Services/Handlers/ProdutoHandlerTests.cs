﻿using Application.AutoMapper;
using Application.Commands.Produtos;
using AutoMapper;
using Domain.Adapters;
using Domain.Entities;
using Domain.Notificacoes;
using Infrastructure.Tests.Adapters;
using Infrastructure.Tests.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using System;
using TechChallenge.src.Adapters.Driven.Infra.Repositories;
using TechChallenge.src.Handlers;

namespace Application.Tests.Services.Handlers
{
    public class ProdutoHandlerTests
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ITabelaPrecoRepository _tabelaPrecoRepository;
        private readonly ICategoriaProdutoRepository _categoriaProdutoRepository;
        private readonly INotificador _notificador;
        private readonly IMapper _mapper;
        private readonly ProdutoHandler _produtoHandler;

        private readonly DbContext _dbContext;

        public ProdutoHandlerTests()
        {
            _produtoRepository = IProdutoRepositoryMock.GetMock();
            _tabelaPrecoRepository = ITabelaPrecoRepositoryMock.GetMock();
            _categoriaProdutoRepository = ICategoriaProdutoRepositoryMock.GetMock();
            _notificador = new Notificador();

            _dbContext = DataBaseContextTests.CreateDbContext();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperConfig>());
            _mapper = config.CreateMapper();

            _produtoHandler = new ProdutoHandler(_notificador, _produtoRepository, _tabelaPrecoRepository, _mapper);
        }

        [Fact]
        public async void Produto_DeveRetornarVerdadeiro_QuandoCadastrarNovo()
        {
            //Arrange
            var command = new CadastraProdutoCommand()
            {
                CategoriaProdutoId = Guid.NewGuid(),
                Nome = "Lanche",
                Descricao = "Cadastro do primeiro Lanche",
            };

            //Act
            var result = await _produtoHandler.Handle(command, default);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Produto_DeveRetornarVerdadeiro_QuandoAtualizar()
        {
            //Arrange
            var commandCadastrar = new Mock<CadastraProdutoCommand>();
            var dado = await _produtoHandler.Handle(commandCadastrar.Object, default);

            var command = new AtualizaProdutoCommand()
            {
                Id = dado.Id,
                CategoriaProdutoId = dado.CategoriaProdutoId,
                Nome = dado.Nome,
                Descricao = "Alterando cadastro do primeiro Lanche",
            };

            //Act
            var result = await _produtoHandler.Handle(command, default);

            //Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async void Produto_DeveRetornarFalse_QuandoNaoRemover()
        {
            //Arrange
            var categoria = await new CategoriaProduto().Cadastrar(_categoriaProdutoRepository, "Categoria Lanche");
            var novoDado = await new Produto().Cadastrar(categoria.Id, "Lanche", "Cadastro do primeiro Lanche");

            var command = new DeletaProdutoCommand()
            {
                Id = novoDado.Id,
            };

            //Act
            var result = await _produtoHandler.Handle(command, default);

            //Assert
            Assert.Null(result);
        }
    }
}
