using System.Linq;
using CommanderGQL.Data;
using CommanderGQL.Models;
using HotChocolate;
using HotChocolate.Types;

namespace CommanderGQL.GraphQL.Commands
{
    public class CommandType : ObjectType<Command>
    {
        protected override void Configure(IObjectTypeDescriptor<Command> descriptor)
        {
            descriptor.Description("Represents any executable command");

            descriptor
               .Field(c => c.Platform)
               .ResolveWith<Resolvers>(c => c.GetPlatform(default!, default!))
               .UseDbContext<AppDbContext>()
               .Description("This is the platform to which the command belongs.");

            descriptor.Field(c => c.CommandLine)
            .Description("This is the commandline to run platform.");

            descriptor.Field(c => c.HowTo)
            .Description("This is the how to run platform commandline.");
        }


        private class Resolvers
        {
            public Platform GetPlatform([Parent] Command command, [ScopedService] AppDbContext context)
            {
                return context.Platforms.FirstOrDefault(p => p.Id == command.PlatformId);
            }
        }
    }
}