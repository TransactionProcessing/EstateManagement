using System;
using System.Collections.Generic;
using System.Text;
using EstateModel = EstateManagement.Models.Estate;
using EstateEntity = EstateReporting.Database.Entities.Estate;
using EstateOperatorEntity = EstateReporting.Database.Entities.EstateOperator;
using EstateSecurityUserEntity = EstateReporting.Database.Entities.EstateSecurityUser;

namespace EstateManagement.BusinessLogic.Tests
{
    using EstateAggregate;
    using EstateReporting.Database.Entities;
    using Models;
    using Models.Factories;
    using Shouldly;
    using Testing;
    using Xunit;

    public class ModelFactoryTests
    {
        [Fact]
        public void EstateEntities_ConvertFrom_EstateConverted()
        {
            EstateEntity estate = TestData.EstateEntity;
            List<EstateOperatorEntity> estateOperators = new List<EstateOperatorEntity>
                                                         {
                                                             TestData.EstateOperatorEntity
                                                         };
            List<EstateSecurityUserEntity> estateSecurityUsers = new List<EstateSecurityUserEntity>
                                                                 {
                                                                     TestData.EstateSecurityUserEntity
                                                                 };

            ModelFactory modelFactory = new ModelFactory();

            EstateModel estateModel = modelFactory.ConvertFrom(estate, estateOperators, estateSecurityUsers);

            estateModel.ShouldNotBeNull();
            estateModel.EstateId.ShouldBe(estate.EstateId);
            estateModel.Name.ShouldBe(estate.Name);
            estateModel.Operators.ShouldNotBeNull();
            estateModel.Operators.Count.ShouldBe(estateOperators.Count);
            estateModel.SecurityUsers.ShouldNotBeNull();
            estateModel.SecurityUsers.Count.ShouldBe(estateSecurityUsers.Count);
        }

        [Fact]
        public void EstateEntities_ConvertFrom_NoOperators_EstateConverted()
        {
            EstateEntity estate = TestData.EstateEntity;
            List<EstateOperatorEntity> estateOperators = new List<EstateOperatorEntity>();
            List<EstateSecurityUserEntity> estateSecurityUsers = new List<EstateSecurityUserEntity>
                                                                 {
                                                                     TestData.EstateSecurityUserEntity
                                                                 };

            ModelFactory modelFactory = new ModelFactory();

            EstateModel estateModel = modelFactory.ConvertFrom(estate, estateOperators, estateSecurityUsers);

            estateModel.ShouldNotBeNull();
            estateModel.EstateId.ShouldBe(estate.EstateId);
            estateModel.Name.ShouldBe(estate.Name);
            estateModel.Operators.ShouldBeNull();
            estateModel.SecurityUsers.ShouldNotBeNull();
            estateModel.SecurityUsers.Count.ShouldBe(estateSecurityUsers.Count);
        }

        [Fact]
        public void EstateEntities_ConvertFrom_NullOperators_EstateConverted()
        {
            EstateEntity estate = TestData.EstateEntity;
            List<EstateOperatorEntity> estateOperators = null;
            List<EstateSecurityUserEntity> estateSecurityUsers = new List<EstateSecurityUserEntity>
                                                                 {
                                                                     TestData.EstateSecurityUserEntity
                                                                 };

            ModelFactory modelFactory = new ModelFactory();

            EstateModel estateModel = modelFactory.ConvertFrom(estate, estateOperators, estateSecurityUsers);

            estateModel.ShouldNotBeNull();
            estateModel.EstateId.ShouldBe(estate.EstateId);
            estateModel.Name.ShouldBe(estate.Name);
            estateModel.Operators.ShouldBeNull();
            estateModel.SecurityUsers.ShouldNotBeNull();
            estateModel.SecurityUsers.Count.ShouldBe(estateSecurityUsers.Count);
        }

        [Fact]
        public void EstateEntities_ConvertFrom_NoSecurityUsers_EstateConverted()
        {
            EstateEntity estate = TestData.EstateEntity;
            List<EstateOperatorEntity> estateOperators = new List<EstateOperatorEntity>
                                                         {
                                                             TestData.EstateOperatorEntity
                                                         };
            List<EstateSecurityUserEntity> estateSecurityUsers = null;

            ModelFactory modelFactory = new ModelFactory();

            EstateModel estateModel = modelFactory.ConvertFrom(estate, estateOperators, estateSecurityUsers);

            estateModel.ShouldNotBeNull();
            estateModel.EstateId.ShouldBe(estate.EstateId);
            estateModel.Name.ShouldBe(estate.Name);
            estateModel.Operators.ShouldNotBeNull();
            estateModel.Operators.Count.ShouldBe(estateOperators.Count);
            estateModel.SecurityUsers.ShouldBeNull();
        }

        [Fact]
        public void EstateEntities_ConvertFrom_NullSecurityUsers_EstateConverted()
        {
            EstateEntity estate = TestData.EstateEntity;
            List<EstateOperatorEntity> estateOperators = new List<EstateOperatorEntity>
                                                         {
                                                             TestData.EstateOperatorEntity
                                                         };
            List<EstateSecurityUserEntity> estateSecurityUsers = null;

            ModelFactory modelFactory = new ModelFactory();

            EstateModel estateModel = modelFactory.ConvertFrom(estate, estateOperators, estateSecurityUsers);

            estateModel.ShouldNotBeNull();
            estateModel.EstateId.ShouldBe(estate.EstateId);
            estateModel.Name.ShouldBe(estate.Name);
            estateModel.Operators.ShouldNotBeNull();
            estateModel.Operators.Count.ShouldBe(estateOperators.Count);
            estateModel.SecurityUsers.ShouldBeNull();
        }
    }
}
