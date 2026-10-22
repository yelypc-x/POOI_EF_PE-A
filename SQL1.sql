USE ExamenFinal;
GO

CREATE TABLE tb_paises (
    idpais  char(3) PRIMARY KEY,
    nompais varchar(50) NOT NULL
);
GO

INSERT INTO tb_paises VALUES
('PER','Perú'),('ARG','Argentina'),('CHL','Chile'),
('COL','Colombia'),('ECU','Ecuador'),('MEX','México');
GO

CREATE TABLE tb_Consumidor (
    dnicont   char(8)      PRIMARY KEY,
    nomcont   varchar(40)  NOT NULL,
    apecont   varchar(40)  NOT NULL,
    emailcont varchar(250) UNIQUE,
    idpais    char(3)      REFERENCES tb_paises(idpais)
);
GO


------------------------

CREATE PROCEDURE usp_agrega_consumidor
    @dnicont   varchar(8),
    @nomcont   varchar(40),
    @apecont   varchar(40),
    @emailcont varchar(250),
    @idpais    varchar(3),
    @mensaje   varchar(100) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- DNI numérico y de 8 dígitos
    IF @dnicont IS NULL OR LEN(@dnicont) <> 8 OR @dnicont LIKE '%[^0-9]%'
    BEGIN
        SET @mensaje = 'Error: el DNI debe ser numérico y de 8 dígitos';
        RETURN;
    END

    -- Nombre y apellido no vacíos
    IF @nomcont IS NULL OR LTRIM(RTRIM(@nomcont)) = ''
    BEGIN
        SET @mensaje = 'Error: el nombre no debe estar vacío';
        RETURN;
    END
    IF @apecont IS NULL OR LTRIM(RTRIM(@apecont)) = ''
    BEGIN
        SET @mensaje = 'Error: el apellido no debe estar vacío';
        RETURN;
    END

    -- País tipo string (solo letras) y existente
    IF @idpais IS NULL OR @idpais = '' OR @idpais LIKE '%[^A-Za-z]%'
    BEGIN
        SET @mensaje = 'Error: el país debe ser tipo string';
        RETURN;
    END
    IF NOT EXISTS (SELECT 1 FROM tb_paises WHERE idpais = @idpais)
    BEGIN
        SET @mensaje = 'Error: el país no existe';
        RETURN;
    END

    -- Duplicados
    IF EXISTS (SELECT 1 FROM tb_Consumidor WHERE dnicont = @dnicont)
    BEGIN
        SET @mensaje = 'Error: ya existe un consumidor con ese DNI';
        RETURN;
    END
    IF EXISTS (SELECT 1 FROM tb_Consumidor WHERE emailcont = @emailcont)
    BEGIN
        SET @mensaje = 'Error: el email ya está registrado';
        RETURN;
    END

    INSERT INTO tb_Consumidor (dnicont, nomcont, apecont, emailcont, idpais)
    VALUES (@dnicont, LTRIM(RTRIM(@nomcont)), LTRIM(RTRIM(@apecont)), @emailcont, @idpais);

    SET @mensaje = 'Consumidor registrado correctamente';
END
GO

----------------------------------

CREATE PROCEDURE usp_eliminar_consumidor
    @dnicont varchar(8),
    @mensaje varchar(100) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM tb_Consumidor WHERE dnicont = @dnicont)
    BEGIN
        SET @mensaje = 'Error: el DNI no existe en la tabla';
        RETURN;
    END

    DELETE FROM tb_Consumidor WHERE dnicont = @dnicont;
    SET @mensaje = 'Consumidor eliminado correctamente';
END
GO
-------------------------

CREATE PROCEDURE usp_listar_consumidor
AS
BEGIN
    SET NOCOUNT ON;
    SELECT dnicont, nomcont, apecont, emailcont, idpais
    FROM tb_Consumidor
    ORDER BY apecont;
END
GO
----------------------------------
