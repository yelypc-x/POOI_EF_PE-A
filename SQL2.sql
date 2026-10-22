USE ExamenFinal;
GO

-- Buscar un consumidor por DNI (sirve para Update y Details)
CREATE PROCEDURE usp_buscar_consumidor
    @dnicont varchar(8)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT dnicont, nomcont, apecont, emailcont, idpais
    FROM tb_Consumidor
    WHERE dnicont = @dnicont;
END
GO

-- Actualizar consumidor
CREATE PROCEDURE usp_actualiza_consumidor
    @dnicont   varchar(8),
    @nomcont   varchar(40),
    @apecont   varchar(40),
    @emailcont varchar(250),
    @idpais    varchar(3),
    @mensaje   varchar(100) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    IF NOT EXISTS (SELECT 1 FROM tb_Consumidor WHERE dnicont = @dnicont)
    BEGIN
        SET @mensaje = 'Error: el DNI no existe en la tabla';
        RETURN;
    END

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

    -- El email no debe pertenecer a otro consumidor
    IF EXISTS (SELECT 1 FROM tb_Consumidor
               WHERE emailcont = @emailcont AND dnicont <> @dnicont)
    BEGIN
        SET @mensaje = 'Error: el email ya está registrado en otro consumidor';
        RETURN;
    END

    UPDATE tb_Consumidor
    SET nomcont   = LTRIM(RTRIM(@nomcont)),
        apecont   = LTRIM(RTRIM(@apecont)),
        emailcont = @emailcont,
        idpais    = @idpais
    WHERE dnicont = @dnicont;

    SET @mensaje = 'Consumidor actualizado correctamente';
END
GO


DECLARE @m varchar(100);
EXEC usp_buscar_consumidor '12345678';
EXEC usp_actualiza_consumidor '12345678','Nayely','Peña','nuevo@mail.com','PER', @m OUTPUT;
PRINT @m;