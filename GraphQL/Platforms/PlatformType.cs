using System.Linq;
using CommanderGQL.Data;
using CommanderGQL.Models;
using HotChocolate;
using HotChocolate.Types;

namespace CommanderGQL.GraphQL.Platforms
{
    public class PlatformType : ObjectType<Platform>
    {
        protected override void Configure(IObjectTypeDescriptor<Platform> descriptor)
        {
            descriptor.Description("Represent any software or service that has a command line interface.");

            descriptor.Field(
                p => p.LicenceKey
            ).Ignore();

            descriptor.Field(p=> p.Commands)
            .ResolveWith<Resolvers>(p=> p.GetCommands(default!,default!))
            .UseDbContext<AppDbContext>()
            .Description("This is the list of avaliable commands for this platform");
        }
        private class Resolvers
        {
            public IQueryable<Command> GetCommands([Parent]Platform platform, [ScopedService] AppDbContext context)
            {
                return context.Commands.Where(p => p.PlatformId == platform.Id);
            }
        }
    }
}