-- REFER TO: https://nielsberglund.com/2017/07/23/sql-server-2017-sqlclr---whitelisting-assemblies/

sp_configure 'show advanced options', 1
reconfigure
go
sp_configure 'clr_enabled', 1
reconfigure
go
sp_configure
go
----------------------------------
use master
go

drop database if exists DeployDB
drop database if exists TrustedAsmDB
go

create database DeployDB
go

alter database DeployDB
set trustworthy on
go

create database TrustedAsmDB
go

use DeployDB
go
create assembly LSREGR
	authorization dbo
	from 'C:\source\LSREGR\bin\x64\Debug\LSREGR.dll'
go
select * from sys.assemblies
go
use master
go
declare @asmBin varbinary(max) = /* add binary here */ 
declare @hash varbinary(64)

select @hash = HASHBYTES('SHA2_512', @asmBin)
declare @clrName nvarchar(4000) = 'lsregr, version=0.0.0.0, culture=neutral, publickeytoken=null, processorarchitecture=amd64'
exec sp_add_trusted_assembly @hash = @hash, 
	@description = @clrName
go
create schema Aggregates;
go

create assembly LSREGR
	authorization dbo
	from 'C:\source\LSREGR\bin\x64\Debug\LSREGR.dll'
	with PERMISSION_SET = SAFE;
go

create aggregate LSREGR_SLOPE (@x float, @y float) returns float
	external name LSREGR.LSREGR_SLOPE
go
create aggregate LSREGR_INTERCEPT (@x float, @y float) returns float
	external name LSREGR.LSREGR_INTERCEPT
go
create aggregate LSREGR_COUNT (@x float, @y float) returns float
	external name LSREGR.LSREGR_COUNT
go
create aggregate LSREGR_R2 (@x float, @y float) returns float
	external name LSREGR.LSREGR_R2
go
create aggregate LSREGR_AVGX (@x float, @y float) returns float
	external name LSREGR.LSREGR_AVGX
go
create aggregate LSREGR_AVGY (@x float, @y float) returns float
	external name LSREGR.LSREGR_AVGY
go
create aggregate LSREGR_SXX (@x float, @y float) returns float
	external name LSREGR.LSREGR_SXX
go
create aggregate LSREGR_SYY (@x float, @y float) returns float
	external name LSREGR.LSREGR_SYY
go
create aggregate LSREGR_SXY (@x float, @y float) returns float
	external name LSREGR.LSREGR_SXY
go
drop aggregate LSREGR_SLOPE
drop aggregate LSREGR_INTERCEPT
drop aggregate LSREGR_COUNT
drop aggregate LSREGR_R2
drop aggregate LSREGR_AVGX
drop aggregate LSREGR_AVGY
drop aggregate LSREGR_SXX
drop aggregate LSREGR_SYY
drop aggregate LSREGR_SXY
drop assembly LSREGR
go
create assembly LSREGR
	authorization dbo
	from 'C:\source\LSREGR\bin\x64\Debug\LSREGR.dll'
go

--drop table LifeExpectancy
--go
--create table LifeExpectancy
--(
--	country nvarchar(max) not null,
--	year int not null,
--	lifeExpectancy float not null
--)
--go

--truncate table LifeExpectancy


--select R2 = dbo.LSREGR_R2(year, lifeExpectancy), INTERCEPT = dbo.LSREGR_INTERCEPT(year, lifeExpectancy), SLOPE = dbo.LSREGR_SLOPE(year, lifeExpectancy), country from LifeExpectancy group by country

--;with data as 
--(
--	select country, lifeExpectancy, x = RANK() over (partition by country order by year) from LifeExpectancy
--) select R2 = dbo.LSREGR_R2(x, lifeExpectancy), INTERCEPT =  dbo.LSREGR_INTERCEPT(x, lifeExpectancy), SLOPE = dbo.LSREGR_SLOPE(x, lifeExpectancy), country from data group by country
