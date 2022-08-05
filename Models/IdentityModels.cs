using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using LODApi.Areas.GLOD.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace LODApi.Models
{
    // Para agregar datos de perfil del usuario, agregue más propiedades a su clase ApplicationUser. Visite https://go.microsoft.com/fwlink/?LinkID=317594 para obtener más información.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Tenga en cuenta que el valor de authenticationType debe coincidir con el definido en CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Agregar aquí notificaciones personalizadas de usuario
            return userIdentity;
        }

        public bool Activo { get; set; }
        public int IdSucursal { get; set; }
        public string Run { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Telefono { get; set; }
        public string Movil { get; set; }
        public string AnexoEmpresa { get; set; }
        public string CargoContacto { get; set; }
        public string DataLetters { get; set; }
        public string RutaImagen { get; set; }
        public string IdCertificado { get; set; }

        [NotMapped]
        public string RutaImg
        {
            get
            {
                if (RutaImagen == null)
                {
                    return string.Empty;
                }
                else
                {
                    return "/Images/Contactos/" + RutaImagen;
                }
            }
        }

        [NotMapped]
        [DisplayName("Nombre Usuario Firmante")]
        public string NombreCompleto
        {
            get
            {
                return Nombres.Split(' ')[0] + " " + Apellidos;
            }
        }

        [NotMapped]
        public string RunToken
        {
            get
            {
                string temp = Run.Replace(".", "").Replace("-", "");
                return temp.Remove(temp.Length - 1, 1);
            }
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("LOD_DB", throwIfV1Schema: false)
        {
        }


        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityUser>().ToTable("SEG_Users", "dbo").Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<ApplicationUser>().ToTable("SEG_Users", "dbo").Property(p => p.Id).HasColumnName("UserId");
            modelBuilder.Entity<IdentityUserRole>().ToTable("SEG_UserRoles", "dbo");
            modelBuilder.Entity<IdentityUserLogin>().ToTable("SEG_UserLogins", "dbo");
            modelBuilder.Entity<IdentityUserClaim>().ToTable("SEG_UserClaims", "dbo");
            modelBuilder.Entity<IdentityRole>().ToTable("SEG_Roles", "dbo");

            var user = modelBuilder.Entity<IdentityUser>().ToTable("SEG_Users", "dbo");
            user.HasMany(u => u.Roles).WithRequired().HasForeignKey(ur => ur.UserId);
            user.HasMany(u => u.Claims).WithRequired().HasForeignKey(uc => uc.UserId);
            user.HasMany(u => u.Logins).WithRequired().HasForeignKey(ul => ul.UserId);
            user.Property(u => u.UserName).IsRequired();
            modelBuilder.Entity<IdentityUserLogin>().HasKey(l => new { l.UserId, l.LoginProvider, l.ProviderKey }).ToTable("SEG_UserLogins", "dbo");

        }

        //REPORTES MENSUALES
        public DbSet<CON_Contratos> CON_Contratos { get; set; }
        //**********************************************************************************

        public DbSet<LOD_Anotaciones> LOD_Anotaciones { get; set; }
        public DbSet<LOD_UserAnotacion> LOD_UserAnotacion { get; set; }
        public DbSet<LOD_UsuariosLod> LOD_UsuariosLod { get; set; }
        public DbSet<LOD_ReferenciasAnot> LOD_ReferenciasAnot { get; set; }
        public DbSet<LOD_LibroObras> LOD_LibroObras { get; set; }
        public DbSet<LOD_docAnotacion> LOD_docAnotacion { get; set; }
        public DbSet<LOD_Carpetas> LOD_Carpetas { get; set; }
        public DbSet<LOD_log> LOD_log { get; set; }
        public DbSet<LOD_PermisosRolesContrato> LOD_PermisosRolesContrato { get; set; }

        ////TABLAS LOCALIZACIÓN
        public virtual DbSet<MAE_ciudad> MAE_ciudad { get; set; }
        public virtual DbSet<MAE_region> MAE_region { get; set; }

        //****************
        //TABLAS AUXILIARES
        public virtual DbSet<MAE_RolesContrato> MAE_RolesContrato { get; set; }
        public virtual DbSet<LOD_RolesCttosContrato> LOD_RolesCttosContrato { get; set; }
        //****************
        //TABLAS PERSONAL

        //****************
        //TABLAS SUJETOS ECONOMICOS
        public virtual DbSet<MAE_sujetoEconomico> MAE_sujetoEconomico { get; set; }
        //TABLAS DOCUMENTOS
        public virtual DbSet<MAE_documentos> MAE_documentos { get; set; }
        public DbSet<MAE_TipoDocumento> MAE_TipoDocumento { get; set; }


        //SEGURIDAD EN ASP****
        public DbSet<MAE_Empresa> MAE_Empresa { get; set; }

        public DbSet<MAE_TipoLOD> MAE_TipoLOD { get; set; }

        public DbSet<MAE_TipoComunicacion> MAE_TipoComunicacion { get; set; }
        public DbSet<MAE_DireccionesMOP> MAE_DireccionesMOP { get; set; }
        public DbSet<MAE_CodSubCom> MAE_CodSubCom { get; set; }

        public DbSet<MAE_SubtipoComunicacion> MAE_SubtipoComunicacion { get; set; }

        public DbSet<MAE_Sucursal> MAE_Sucursal { get; set; }
        public DbSet<MAE_ClassDoc> MAE_ClassDoc { get; set; }

        public DbSet<MAE_ClassOne> MAE_ClassOne { get; set; }

        public DbSet<MAE_ClassTwo> MAE_ClassTwo { get; set; }
        public DbSet<MAIL_Envio> MAIL_Envio { get; set; }

        public DbSet<FORM_InformesItems> FORM_InformesItems { get; set; }
    }
}