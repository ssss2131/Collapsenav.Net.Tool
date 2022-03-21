using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Collapsenav.Net.Tool.Data;
public static class DbContextConnExt
{
    public static Action<DbContextOptionsBuilder> GenBuilder<T>(this T conn, Assembly ass = null) where T : Conn
    {
        if (ass == null)
            return conn switch
            {
                SqliteConn => (DbContextOptionsBuilder option) => option.UseSqlite(conn.ToString()),
                SqlServerConn => (DbContextOptionsBuilder option) => option.UseSqlServer(conn.ToString()),
#if NETSTANDARD2_0
                MysqlConn =>(DbContextOptionsBuilder option) => option.UseMySql(conn.ToString()),
                MariaDbConn =>(DbContextOptionsBuilder option) => option.UseMySql(conn.ToString()),
#else
                MysqlConn => (DbContextOptionsBuilder option) => option.UseMySql(conn.ToString(), new MySqlServerVersion("8.0")),
                MariaDbConn => (DbContextOptionsBuilder option) => option.UseMySql(conn.ToString(), new MySqlServerVersion("8.0")),
#endif
                PgsqlConn => (DbContextOptionsBuilder option) => option.UseSqlite(conn.ToString()),
                _ => null
            };
        var assName = ass?.GetName().Name;
        return conn switch
        {
            SqliteConn => (DbContextOptionsBuilder option) => option.UseSqlite(conn.ToString(), o => o.MigrationsAssembly(assName)),
            SqlServerConn => (DbContextOptionsBuilder option) => option.UseSqlServer(conn.ToString(), o => o.MigrationsAssembly(assName)),
#if NETSTANDARD2_0
            MysqlConn =>(DbContextOptionsBuilder option) => option.UseMySql(conn.ToString(), o => o.MigrationsAssembly(assName)),
            MariaDbConn =>(DbContextOptionsBuilder option) => option.UseMySql(conn.ToString(), o => o.MigrationsAssembly(assName)),
#else
            MysqlConn => (DbContextOptionsBuilder option) => option.UseMySql(conn.ToString(), new MySqlServerVersion("8.0"), o => o.MigrationsAssembly(assName)),
            MariaDbConn => (DbContextOptionsBuilder option) => option.UseMySql(conn.ToString(), new MySqlServerVersion("8.0"), o => o.MigrationsAssembly(assName)),
#endif
            PgsqlConn => (DbContextOptionsBuilder option) => option.UseSqlite(conn.ToString(), o => o.MigrationsAssembly(assName)),
            _ => null
        };
    }
    public static IServiceCollection AddDbContext<T, E>(this IServiceCollection services, E conn, Assembly ass = null) where T : DbContext where E : Conn
    {
        services.AddDefaultIdGenerator();
        if (typeof(T) == typeof(PgsqlConn))
            BaseEntity.GetNow = () => DateTime.UtcNow;
        return services.AddDbContext<T>(conn.GenBuilder());
    }
    public static IServiceCollection AddDbContextPool<T, E>(this IServiceCollection services, E conn, Assembly ass = null) where T : DbContext where E : Conn
    {
        services.AddDefaultIdGenerator();
        if (typeof(T) == typeof(PgsqlConn))
            BaseEntity.GetNow = () => DateTime.UtcNow;
        return services.AddDbContextPool<T>(conn.GenBuilder());
    }
    public static IServiceCollection AddSqlite<T>(this IServiceCollection services, SqliteConn conn) where T : DbContext
    {
        return services.AddDbContext<T, SqliteConn>(conn);
    }
    public static IServiceCollection AddSqlServer<T>(this IServiceCollection services, SqlServerConn conn, Assembly ass = null) where T : DbContext
    {
        return services.AddDbContext<T, SqlServerConn>(conn, ass);
    }
    public static IServiceCollection AddMysql<T>(this IServiceCollection services, MysqlConn conn, Assembly ass = null) where T : DbContext
    {
        return services.AddDbContext<T, MysqlConn>(conn, ass);
    }
    public static IServiceCollection AddMariaDb<T>(this IServiceCollection services, MariaDbConn conn, Assembly ass = null) where T : DbContext
    {
        return services.AddDbContext<T, MariaDbConn>(conn, ass);
    }
    public static IServiceCollection AddPgSql<T>(this IServiceCollection services, PgsqlConn conn, Assembly ass = null) where T : DbContext
    {
        return services.AddDbContext<T, PgsqlConn>(conn, ass);
    }


    public static IServiceCollection AddSqlite<T>(this IServiceCollection services, string db) where T : DbContext
    {
        return services.AddDbContext<T, SqliteConn>(new SqliteConn(db));
    }
    public static IServiceCollection AddSqlServer<T>(this IServiceCollection services, string source, int? port, string dataBase, string user, string pwd, Assembly ass = null) where T : DbContext
    {
        return services.AddDbContext<T, SqlServerConn>(new SqlServerConn(source, port, dataBase, user, pwd), ass);
    }
    public static IServiceCollection AddMysql<T>(this IServiceCollection services, string source, int? port, string dataBase, string user, string pwd, Assembly ass = null) where T : DbContext
    {
        return services.AddDbContext<T, MysqlConn>(new MysqlConn(source, port, dataBase, user, pwd), ass);
    }
    public static IServiceCollection AddMariaDb<T>(this IServiceCollection services, string source, int? port, string dataBase, string user, string pwd, Assembly ass = null) where T : DbContext
    {
        return services.AddDbContext<T, MariaDbConn>(new MariaDbConn(source, port, dataBase, user, pwd), ass);
    }
    public static IServiceCollection AddPgSql<T>(this IServiceCollection services, string source, int? port, string dataBase, string user, string pwd, Assembly ass = null) where T : DbContext
    {
        return services.AddDbContext<T, PgsqlConn>(new PgsqlConn(source, port, dataBase, user, pwd), ass);
    }


    public static IServiceCollection AddSqlitePool<T>(this IServiceCollection services, SqliteConn conn, Assembly ass = null) where T : DbContext
    {
        return services.AddDbContextPool<T, SqliteConn>(conn, ass);
    }
    public static IServiceCollection AddSqlServerPool<T>(this IServiceCollection services, SqlServerConn conn, Assembly ass = null) where T : DbContext
    {
        return services.AddDbContextPool<T, SqlServerConn>(conn, ass);
    }
    public static IServiceCollection AddMysqlPool<T>(this IServiceCollection services, MysqlConn conn, Assembly ass = null) where T : DbContext
    {
        return services.AddDbContextPool<T, MysqlConn>(conn, ass);
    }
    public static IServiceCollection AddMariaDbPool<T>(this IServiceCollection services, MariaDbConn conn, Assembly ass = null) where T : DbContext
    {
        return services.AddDbContextPool<T, MariaDbConn>(conn, ass);
    }
    public static IServiceCollection AddPgSqlPool<T>(this IServiceCollection services, PgsqlConn conn, Assembly ass = null) where T : DbContext
    {
        return services.AddDbContextPool<T, PgsqlConn>(conn, ass);
    }


    public static IServiceCollection AddSqlitePool<T>(this IServiceCollection services, string db, Assembly ass = null) where T : DbContext
    {
        return services.AddDbContextPool<T, SqliteConn>(new SqliteConn(db), ass);
    }
    public static IServiceCollection AddSqlServerPool<T>(this IServiceCollection services, string source, int? port, string dataBase, string user, string pwd, Assembly ass = null) where T : DbContext
    {
        return services.AddDbContextPool<T, SqlServerConn>(new SqlServerConn(source, port, dataBase, user, pwd), ass);
    }
    public static IServiceCollection AddMysqlPool<T>(this IServiceCollection services, string source, int? port, string dataBase, string user, string pwd, Assembly ass = null) where T : DbContext
    {
        return services.AddDbContextPool<T, MysqlConn>(new MysqlConn(source, port, dataBase, user, pwd), ass);
    }
    public static IServiceCollection AddMariaDbPool<T>(this IServiceCollection services, string source, int? port, string dataBase, string user, string pwd, Assembly ass = null) where T : DbContext
    {
        return services.AddDbContextPool<T, MariaDbConn>(new MariaDbConn(source, port, dataBase, user, pwd), ass);
    }
    public static IServiceCollection AddPgSqlPool<T>(this IServiceCollection services, string source, int? port, string dataBase, string user, string pwd, Assembly ass = null) where T : DbContext
    {
        return services.AddDbContextPool<T, PgsqlConn>(new PgsqlConn(source, port, dataBase, user, pwd), ass);
    }

    /// <summary>
    /// 将 T 注册为默认的 DbContext
    /// </summary>
    public static IServiceCollection AddDefaultDbContext<T>(this IServiceCollection services) where T : DbContext
    {
        services.AddScoped<DbContext, T>();
        return services;
    }
    /// <summary>
    /// 注册默认id生成
    /// </summary>
    /// <remarks>
    /// 暂时只支持 Guid, Guid?, long, long?
    /// </remarks>
    public static IServiceCollection AddDefaultIdGenerator(this IServiceCollection services)
    {
        BaseEntity<Guid>.GetKey ??= () => Guid.NewGuid();
        BaseEntity<Guid?>.GetKey ??= () => Guid.NewGuid();
        BaseEntity<long>.GetKey ??= () => SnowFlake.NextId();
        BaseEntity<long?>.GetKey ??= () => SnowFlake.NextId();
        return services;
    }
}