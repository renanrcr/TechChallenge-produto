﻿using TechChallenge.src.Core.Application.Validations.Clientes.Base;
using TechChallenge.src.Core.Domain.Adapters;

namespace TechChallenge.src.Core.Application.Validations.Clientes
{
    public class DeletaClienteValidation : ClienteBaseValidation
    {
        public DeletaClienteValidation(IClienteRepository clienteRepository) 
            : base(clienteRepository)
        {
            ValidarExisteClienteCadastrado();
            ValidarDataExclusao();
        }
    }
}