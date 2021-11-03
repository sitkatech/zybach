//  IMPORTANT:
//  This file is generated. Your changes will be lost.
//  Use the corresponding partial class for customizations.
//  Source Table: [HangFire].[Server]

using Zybach.Models.DataTransferObjects;

namespace Zybach.EFModels.Entities
{
    public static partial class ServerExtensionMethods
    {
        public static ServerDto AsDto(this Server server)
        {
            var serverDto = new ServerDto()
            {
                Id = server.Id,
                Data = server.Data,
                LastHeartbeat = server.LastHeartbeat
            };
            DoCustomMappings(server, serverDto);
            return serverDto;
        }

        static partial void DoCustomMappings(Server server, ServerDto serverDto);

    }
}