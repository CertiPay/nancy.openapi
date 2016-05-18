﻿using Nancy.OpenApi.Infrastructure;
using Nancy.OpenApi.Models;
using Nancy.OpenApi.Tests.Fakes;
using Nancy.Testing;
using Newtonsoft.Json;
using NUnit.Framework;

namespace Nancy.OpenApi.Tests
{
    public class GivenOpenApiModule
    {
        [Test]
        public void WhenIMakeDocumentationRequestOnDefaultRoute_ThenSwaggerDocumentationIsRendered()
        {
            var browser = new Browser(new Infrastructure.Bootstrapper());
            var response = browser.Get("/api-docs", (with) => { with.HttpRequest(); });

            response.Body["#swagger-ui-container"].ShouldExistOnce();
        }

        [Test]
        public void WhenIMakeSpecificationRequestOnDefaultRoute_ThenJsonSpecificationIsRetrieved()
        {
            var browser = new Browser(new Infrastructure.Bootstrapper());
            var response = browser.Get("/swagger.json", (with) => { with.Header("Accept", "application/json"); });
            var specs = JsonConvert.DeserializeObject<ApiSpecification>(response.Body.AsString());

            Assert.That(specs, Is.Not.Null);
            Assert.That(specs.Swagger, Is.EqualTo("2.0"));
        }

        [Test]
        public void WhenIHaveAModuleWithConfiguredRoutes_ThenJsonSpecificationRepresentsConfiguredRoutes()
        {
            var browser = new Browser(new Infrastructure.Bootstrapper());
            var response = browser.Get("/swagger.json", (with) => { with.Header("Accept", "application/json"); });
            var specs = JsonConvert.DeserializeObject<ApiSpecification>(response.Body.AsString());
            var path = specs.Paths["/api/test"];

            Assert.That(specs, Is.Not.Null);
            Assert.That(specs.Swagger, Is.EqualTo("2.0"));
            Assert.That(specs.Info.Contact.Name, Is.EqualTo(_apiDescription.ContactName));
            Assert.That(specs.Info.Contact.Email, Is.EqualTo(_apiDescription.ContactEmail));
            Assert.That(specs.Info.Contact.Url, Is.EqualTo(_apiDescription.ContactUrl));
            Assert.That(specs.Info.Description, Is.EqualTo(_apiDescription.Description));
            Assert.That(specs.Info.Licence.Name, Is.EqualTo(_apiDescription.LicenseName));
            Assert.That(specs.Info.Licence.Url, Is.EqualTo(_apiDescription.LicenseUrl));
            Assert.That(specs.Info.TermsOfService, Is.EqualTo(_apiDescription.TermsOfService));
            Assert.That(specs.Info.Title, Is.EqualTo(_apiDescription.Title));
            Assert.That(specs.Info.Version, Is.EqualTo(_apiDescription.Version));

            Assert.That(path, Is.Not.Null);
            Assert.That(path["post"].Description, Is.EqualTo(_metadata.PostMetadata.Description));
            Assert.That(path["post"].Summary, Is.EqualTo(_metadata.PostMetadata.Summary));
            Assert.That(path["post"].OperationId, Is.EqualTo(_metadata.PostMetadata.OperationId));
        }

        private readonly IApiDescription _apiDescription = new FakeApiDescription();
        private readonly FakeMetadata _metadata = new FakeMetadata();
    }
}