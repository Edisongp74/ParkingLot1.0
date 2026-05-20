using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ParkingLot1._0.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAuditLogsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Este condicional evita que la migración falle si la tabla ya existiera en la máquina de alguien
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[dbo].[AuditLogs]') IS NULL
                BEGIN
                    CREATE TABLE [dbo].[AuditLogs] (
                        [Id] INT IDENTITY (1, 1) NOT NULL,
                        [Usuario] NVARCHAR (MAX) NOT NULL,
                        [Accion] NVARCHAR (MAX) NOT NULL,
                        [Detalle] NVARCHAR (MAX) NOT NULL,
                        [ControllerName] NVARCHAR (MAX) NOT NULL,
                        [ActionName] NVARCHAR (MAX) NOT NULL,
                        [IpAddress] NVARCHAR (MAX) NOT NULL,
                        [FechaRegistro] DATETIME2 (7) NOT NULL,
                        CONSTRAINT [PK_AuditLogs] PRIMARY KEY CLUSTERED ([Id] ASC)
                    );
                END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // En caso de querer revertir la migración, borra la tabla de forma segura
            migrationBuilder.Sql(@"
                IF OBJECT_ID(N'[dbo].[AuditLogs]') IS NOT NULL
                BEGIN
                    DROP TABLE [dbo].[AuditLogs];
                END;
            ");
        }
    }
}
