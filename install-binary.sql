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
declare @asmBin varbinary(max) = 0x4D5A90000300000004000000FFFF0000B800000000000000400000000000000000000000000000000000000000000000000000000000000000000000800000000E1FBA0E00B409CD21B8014CCD21546869732070726F6772616D2063616E6E6F742062652072756E20696E20444F53206D6F64652E0D0D0A240000000000000050450000648602005137F89B0000000000000000F00022200B023000001000000004000000000000000000000020000000000080010000000020000000020000040000000000000006000000000000000060000000020000000000000300608500004000000000000040000000000000000010000000000000200000000000000000000010000000000000000000000000000000000000000040000068030000000000000000000000000000000000000000000000000000082F0000380000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000002000004800000000000000000000002E74657874000000820F0000002000000010000000020000000000000000000000000000200000602E727372630000006803000000400000000400000012000000000000000000000000000040000040000000000000000000000000000000000000000000000000000000000000000000000000000000004800000002000500D0230000380B000001000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001E027B010000042A2202037D010000042A1E027B020000042A2202037D020000042A1E027B030000042A2202037D030000042A1E027B040000042A2202037D040000042A1E027B050000042A2202037D050000042AFE000216281200000A280200000600027E1300000A280400000600027E1300000A280600000600027E1300000A280800000600027E1300000A280A000006002A000000133004008F00000001000011000F01281400000A2D090F02281400000A2B01170A062C0400002B72000202280100000617281200000A281500000A280200000600020228030000060304281600000A281700000A280400000600020228050000060303281600000A281700000A2806000006000202280700000603281700000A2808000006000202280900000604281700000A280A00000600002A00133003003001000002000011000F01280100000616281200000A281800000A13041104281900000A2D1A11040F0128030000067E1300000A281A00000A281B00000A2B0211040D09281900000A2D19090F0128050000067E1300000A281A00000A281B00000A2B01090C08281900000A2D19080F0128070000067E1300000A281A00000A281B00000A2B01080B07281900000A2D19070F0128090000067E1300000A281A00000A281B00000A2B0107281900000A0A062C0400002B7F00020228010000060F012801000006281500000A280200000600020228030000060F012803000006281700000A280400000600020228050000060F012805000006281700000A280600000600020228070000060F012807000006281700000A280800000600020228090000060F012809000006281700000A280A00000600002A133004000C010000030000110002280100000616281200000A281800000A0D09281900000A2D18090228030000067E1300000A281A00000A281B00000A2B01090C08281900000A2D18080228050000067E1300000A281A00000A281B00000A2B01080B07281900000A2D18070228070000067E1300000A281A00000A281B00000A2B01070A06281900000A2D18060228090000067E1300000A281A00000A281B00000A2B0106281900000A2D5F022801000006281C00000A022803000006281600000A022807000006022809000006281600000A281D00000A022801000006281C00000A022805000006281600000A022807000006022807000006281600000A281D00000A281E00000A2B057E1F00000A13042B0011042A42534A4201000100000000000C00000076342E302E33303331390000000005006C00000048040000237E0000B40400004C04000023537472696E67730000000000090000040000002355530004090000100000002347554944000000140900002402000023426C6F6200000000000000020000015715A2010900000000FA013300160000010000001800000002000000050000000E000000080000001F000000230000000300000001000000050000000A00000001000000020000000000D00201000000000006001D029C0306008A029C03060015016A030F00BC0300000600580113030600000213030600E101130306007102130306003D021303060056021303060090011303060044017D03060007017D030600C40113030600AB01B6020A00E00349030A006F0149030600B600E6020600EC009C030600D5006A03060029016A030A000100CB030A00AC00CB030A00ED02CB03000000000A0000000000010001000921100013000000490001000100010041009400010091002C00010068002C00010054002C0001007D002C00482000000000860819009800010050200000000086081F009D00010059200000000081083C04A300020061200000000081084404A80002006A200000000086080604A300030072200000000086080E04A80003007B20000000008608F803A30004008320000000008608FF03A80004008C200000000086081604A300050094200000000086081D04A80005009D20000000008600F30306000600E020000000008600C000AE0006007C21000000008600A600B6000800B822000000008600CB00A300090000000100A80200000100A80200000100A80200000100A80200000100A802000001001404000002004A04000001003603090064030100110064030600190064030A00290064031000310064031000390064031000410064031000490064031000510064031000590064031000610064031500690064031000710064031000790064031000890064031A00990064030600A90064032000B100E7032600B90031032C00B900DB023400B10025033800B90024044100B90025034100B10030045600C100AE025F00B90030046500C1003C036E00B900E7038400B90004034100B900F8024100B900E1022C0020008300A60121008300A60121008B00AB012E000B00C6002E001300CF002E001B00EE002E002300F7002E002B0003012E00330003012E003B0003012E004300F7002E004B0009012E00530003012E005B0003012E00630021012E006B004B012E007300580140008300A60141008300A60141008B00AB0143007B00B40160008300A60161008300A60161008B00AB0180008300A60181008300A60181008B00AB01A0008300A601A1008300A601A1008B00AB01C0008300A601E0008300A60100018300A60120018300A60140018300A60130004A0077000200010000002300BC0000004804C10000001204C10000000304C10000002104C100020001000300010002000300020003000500010004000500020005000700010006000700020007000900010008000900020009000B0001000A000B00048000000100000000000000000000000000250000000400000000000000000000008B003800000000000400000000000000000000008B002C000000000000000053716C496E743136003C4D6F64756C653E00534C4F5045006765745F4E007365745F4E004C53524547520053797374656D2E44617461006D73636F726C6962003C4E3E6B5F5F4261636B696E674669656C64003C53783E6B5F5F4261636B696E674669656C64003C5378783E6B5F5F4261636B696E674669656C64003C53793E6B5F5F4261636B696E674669656C64003C5378793E6B5F5F4261636B696E674669656C64004D657267650053716C446F75626C650056616C75655479706500416363756D756C617465005465726D696E61746500446562756767657242726F777361626C65537461746500436F6D70696C657247656E65726174656441747472696275746500477569644174747269627574650044656275676761626C6541747472696275746500446562756767657242726F777361626C6541747472696275746500436F6D56697369626C6541747472696275746500417373656D626C795469746C654174747269627574650053716C55736572446566696E656441676772656761746541747472696275746500417373656D626C7954726164656D61726B417474726962757465005461726765744672616D65776F726B41747472696275746500417373656D626C7946696C6556657273696F6E41747472696275746500417373656D626C79436F6E66696775726174696F6E41747472696275746500417373656D626C794465736372697074696F6E41747472696275746500436F6D70696C6174696F6E52656C61786174696F6E7341747472696275746500417373656D626C7950726F6475637441747472696275746500417373656D626C79436F7079726967687441747472696275746500417373656D626C79436F6D70616E794174747269627574650052756E74696D65436F6D7061746962696C6974794174747269627574650076616C7565006F705F547275650053797374656D2E52756E74696D652E56657273696F6E696E67004C53524547522E646C6C006765745F49734E756C6C0053797374656D0053716C426F6F6C65616E006F705F4469766973696F6E006F705F5375627472616374696F6E0053797374656D2E5265666C656374696F6E006F705F4164646974696F6E005A65726F0067726F7570006F705F426974776973654F72004D6963726F736F66742E53716C5365727665722E536572766572002E63746F720053797374656D2E446961676E6F73746963730053797374656D2E52756E74696D652E496E7465726F7053657276696365730053797374656D2E52756E74696D652E436F6D70696C6572536572766963657300446562756767696E674D6F6465730053797374656D2E446174612E53716C547970657300466F726D6174006F705F496D706C6963697400496E6974006765745F5378007365745F5378006765745F537878007365745F537878006765745F5379007365745F5379006F705F4D756C7469706C79006F705F457175616C697479006765745F537879007365745F5378790000000000C356DCCCBD779C45B1AE0308928AE08C00042001010803200001052001011111042001010E04200101020520010111410520010111510500011159060306115D0307010203200002080002115911591159080002115D115D115D0B07050211611161116111610800021161115911590500010211610800021161115D115D0800021161116111610C07051161116111611161115D060001115D115908B77A5C561934E089030611590420001159052001011159042000115D05200101115D07200201115D115D0520010111080428001159042800115D0801000800000000001E01000100540216577261704E6F6E457863657074696F6E5468726F7773010801000701000000000B0100064C5352454752000005010000000017010012436F7079726967687420C2A920203230323100002901002439333432613361642D636361352D343634662D383163302D39383336353362376535303700000C010007312E302E302E3000004D01001C2E4E45544672616D65776F726B2C56657273696F6E3D76342E372E320100540E144672616D65776F726B446973706C61794E616D65142E4E4554204672616D65776F726B20342E372E3204010000000801000000000000006D01000100000005005402174973496E76617269616E74546F4475706C696361746573005402124973496E76617269616E74546F4E756C6C73015402124973496E76617269616E74546F4F726465720154020D49734E756C6C4966456D70747901540E044E616D6505534C4F50450000000000008ABDA2F8000000000200000042000000402F0000401100000000000000000000000000001000000000000000000000000000000052534453CAEF093F3E784A4B81FD78B7D4E25DBF01000000433A5C736F757263655C4C53524547525C6F626A5C7836345C44656275675C4C53524547522E70646200000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000001001000000018000080000000000000000000000000000001000100000030000080000000000000000000000000000001000000000048000000584000000C03000000000000000000000C0334000000560053005F00560045005200530049004F004E005F0049004E0046004F0000000000BD04EFFE00000100000001000000000000000100000000003F000000000000000400000002000000000000000000000000000000440000000100560061007200460069006C00650049006E0066006F00000000002400040000005400720061006E0073006C006100740069006F006E00000000000000B0046C020000010053007400720069006E006700460069006C00650049006E0066006F0000004802000001003000300030003000300034006200300000001A000100010043006F006D006D0065006E007400730000000000000022000100010043006F006D00700061006E0079004E0061006D0065000000000000000000360007000100460069006C0065004400650073006300720069007000740069006F006E00000000004C005300520045004700520000000000300008000100460069006C006500560065007200730069006F006E000000000031002E0030002E0030002E003000000036000B00010049006E007400650072006E0061006C004E0061006D00650000004C00530052004500470052002E0064006C006C00000000004800120001004C006500670061006C0043006F007000790072006900670068007400000043006F0070007900720069006700680074002000A90020002000320030003200310000002A00010001004C006500670061006C00540072006100640065006D00610072006B00730000000000000000003E000B0001004F0072006900670069006E0061006C00460069006C0065006E0061006D00650000004C00530052004500470052002E0064006C006C00000000002E0007000100500072006F0064007500630074004E0061006D006500000000004C005300520045004700520000000000340008000100500072006F006400750063007400560065007200730069006F006E00000031002E0030002E0030002E003000000038000800010041007300730065006D0062006C0079002000560065007200730069006F006E00000031002E0030002E0030002E0030000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000000
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

create aggregate Aggregates.SLOPE (@x float, @y float) returns float
	external name LSREGR.[SLOPE]
go
drop aggregate Aggregates.SLOPE
drop assembly LSREGR
go
