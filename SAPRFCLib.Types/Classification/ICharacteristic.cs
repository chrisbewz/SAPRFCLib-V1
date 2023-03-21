using RuntimeHelpers.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAPRFCLib.Types.Classification;

public interface IDescriptor
{
    string InternalName { get; }
    string ExternalName { get; }
}

public interface IClassDescriptor : IDescriptor
{
    int InternalNumber { get; }
}

public interface ICharacteristicDescriptor : IDescriptor
{
    
}
public interface ICharacteristicContainer
    {
        IEnumerable<CharacteristicDescriptions> Descriptions { get;}
        IEnumerable<References> References { get; }
        IEnumerable<Restrictions> Restrictions { get; }
        IEnumerable<CharValues> CharacterValues { get;}
        IEnumerable<NumValues> NumericalValues { get; }
        IEnumerable<Details> GeneralDetails { get; }
        IEnumerable<ValueDescriptions> ValueDescriptions { get; }
    }

public abstract class CharacteristicInformationStructure
{

    [Aliases("descriptions", "desc")]
    [Description("CharacteristicInformation fields individual descriptions")]
    public abstract IEnumerable<CharacteristicDescriptions> SetDescription();

    [Aliases("references", "ref", "refs")]
    [Description("SAP check table and field for characteristic")]
    public abstract  IEnumerable<References> SetReference();

    [Aliases("restrictions", "restrict")]
    [Description("Restricted values for characteristic")]
    public abstract  IEnumerable<Restrictions> SetRestriction();

    [Aliases("charvalues", "charval")]
    [Description("CHAR ABAP type fields and it's values in characteristic information")]
    public abstract  IEnumerable<CharValues> SetCharValue();

    [Aliases("numvalues", "numval")]
    [Description("NUMERICAL ABAP type fields and it's values in characteristic information")]
    public abstract  IEnumerable<NumValues> SetNumericalValues();

    [Aliases("generaldetails", "details")]
    [Description("CharacteristicInformation specific properties")]
    public abstract  IEnumerable<Details> SetGeneralInformation();

    [Aliases("valuedescriptions", "valdescriptions")]
    [Description("CharacteristicInformation individual value descriptions")]
    public abstract  IEnumerable<ValueDescriptions> SetValueDescriptions();

}
    [Description("Container for all general details for a single characteristic as it's external and internal database language definitions," +
                 " descriptions and additional headers." +
                 "This object contains the general information for a unique characteristic defined in all existent languages in database whose it is contained by properties LANGUAGE_INT ad LANGUAGE_ISO.")]
public class CharacteristicDescriptions :ICharacteristic
{
    public CharacteristicDescriptions()
    {

    }

    [Aliases("LangInt")]
    [ColumnNames("LANGUAGE_INT")]
    public char LANG_SPRAS { get; set; }

    [Aliases("LangIso")]
    [ColumnNames("LANGUAGE_ISO")]
    public string LANG_ISO { get; set; }

    [Aliases("Desc")]
    [ColumnNames("DESCRIPTION")]
    public string DESCRIPTION { get; set; }

    [Aliases("FirstHeader")]
    [ColumnNames("HEADER1")]
    public string HEADER1 { get; set; }

    [Aliases("SecondHeader")]
    [ColumnNames("HEADER2")]
    public string HEADER2 { get; set; }
}
    
    
[Description("Container for all value details for a single characteristic whose ABAP characteristic type is defined as ABAP numeric type." +
                 "The value description is contained by property DESCRIPTION, where it's unique match code is defined in VALUE_CHAR." +
                 "This class provides language properties to handle the foreign descriptions definitions for a ABAP numeric characteristic value (If it exists).")]
public class ValueDescriptions :ICharacteristic
{
    public ValueDescriptions()
    {

    }

    [Aliases("LangInt")]
    [ColumnNames("LANGUAGE_INT")]
    public char LANG_SPRAS { get; set; }

    [Aliases("LangIso")]
    [ColumnNames("LANGUAGE_ISO")]
    public string LANG_ISO { get; set; }

    [Aliases("CharValue")]
    [ColumnNames("VALUE_CHAR")]
    public string VALUE_CHAR { get; set; }

    [Aliases("Desc")]
    [ColumnNames("DESCRIPTION")]
    public string DESCRIPTION { get; set; }
}

[Description("Contains the information about location and reference tables for the queried characteristic and any dependent characteristic." +
             "As SAP Database contains a reference table for a value referring to a dependent field, such as CABN table for characteristics single information or AUSP for general classification," +
             "if any characteristic queried has a dependent table to search for it's values, this table is declared using REFERENCE_TABLE as te field is delcared in REFERENCE_FIELD.")]
public class References :ICharacteristic
{
    public References()
    {

    }
    [Aliases("CheckTable")]
    [ColumnNames("REFERENCE_TABLE")]
    public string REF_TABLE { get; set; }

    [Aliases("CheckField")]
    [ColumnNames("REFERENCE_FIELD")]
    public string REF_FIELD { get; set; }
}

[Description("Container for class restrictions information for a single characteristic and it's dependent characteristics.")]
public class Restrictions :ICharacteristic
{
    public Restrictions()
    {

    }
    [Aliases("ClassInternalID")]
    [ColumnNames("CLASS_TYPE")]
    public string CLASS_TYPE { get; set; }
}
[Description("Container for storing ABAP CHAR code referring to de characteristic language dependent external description details for a single characteristic.")]
public class CharValues :ICharacteristic
{
    public CharValues()
    {

    }
    [Aliases("CharValue")]
    [ColumnNames("VALUE_CHAR")]
    public string VALUE_CHAR { get; set; }

    [Aliases("CharValueLong")]
    [ColumnNames("VALUE_CHAR_HIGH")]
    public string VALUE_CHAR_HIGH { get; set; }

    [Aliases("Default")]
    [ColumnNames("DEFAULT_VALUE")]
    public string DEFAULT_VALUE { get; set; }

    [Aliases("DocNumber")]
    [ColumnNames("DOCUMENT_NO")]
    public string DOCUMENT_NO { get; set; }

    [Aliases("DocType")]
    [ColumnNames("DOCUMENT_TYPE")]
    public string DOCUMENT_TYPE { get; set; }

    [Aliases("DocPartVersion")]
    [ColumnNames("DOCUMENT_PART")]
    public string DOCUMENT_PART { get; set; }

    [Aliases("DocVersion")]
    [ColumnNames("DOCUMENT_VERSION")]
    public string DOCUMENT_VERSION { get; set; }
}
[Description("Container for storing ABAP NUM code referring to de characteristic language dependent external description details for a single characteristic.")]
public class NumValues : ICharacteristic
{
    public NumValues()
    {

    }


    [Aliases("ValFrom")]
    [ColumnNames("VALUE_FROM")]
    public string VALUE_FROM { get; set; }

    [Aliases("ValTo")]
    [ColumnNames("VALUE_TO")]
    public string VALUE_TO { get; set; }

    [Aliases("ValRelated")]
    [ColumnNames("VALUE_RELATION")]
    public char VALUE_RELATION { get; set; }

    [Aliases("UnitFrom")]
    [ColumnNames("UNIT_FROM")]
    public string UNIT_FROM { get; set; }

    [Aliases("UnitTo")]
    [ColumnNames("UNIT_TO")]
    public string UNIT_TO { get; set; }

    [Aliases("UnitFromIso")]
    [ColumnNames("UNIT_FROM_ISO")]
    public string UNIT_FROM_ISO { get; set; }

    [Aliases("UnitToIso")]
    [ColumnNames("UNIT_TO_ISO")]
    public string UNIT_TO_ISO { get; set; }

    [Aliases("Default")]
    [ColumnNames("DEFAULT_VALUE")]
    public string DEFAULT_VALUE { get; set; }

    [Aliases("DocNumber")]
    [ColumnNames("DOCUMENT_NO")]
    public string DOCUMENT_NO { get; set; }

    [Aliases("DocType")]
    [ColumnNames("DOCUMENT_TYPE")]
    public string DOCUMENT_TYPE { get; set; }

    [Aliases("DocPartVersion")]
    [ColumnNames("DOCUMENT_PART")]
    public string DOCUMENT_PART { get; set; }

    [Aliases("DocVersion")]
    [ColumnNames("DOCUMENT_VERSION")]
    public string DOCUMENT_VERSION { get; set; }
}

[Description("Container for storing presentation format of SAP interface fields and it's general definitions like field length, templates for data inserted on field as other format descriptors.")]
public class Details : ICharacteristic
{
    public Details()
    {

    }
    [Aliases("CharacteristicName")]
    [ColumnNames("CHARACT_NAME")]
    public string CHARACT_NAME { get; set; }

    [Aliases("CharacteristicDataType")]
    [ColumnNames("DATA_TYPE")]
    public string DATA_TYPE { get; set; }

    [Aliases("CharacteristicDataLenght")]
    [ColumnNames("LENGTH")]
    public string LENGTH { get; set; }

    [Aliases("CharacteristicDecimalDigits")]
    [ColumnNames("DECIMALS")]
    public string DECIMALS { get; set; }

    [Aliases("CharacteristicIsCaseSensitive")]
    [ColumnNames("CASE_SENSITIV")]
    public string CASE_SENSITIV { get; set; }

    [Aliases("CharacteristicValueExponent")]
    [ColumnNames("EXPONENT_TYPE")]
    public string EXPONENT_TYPE { get; set; }

    [Aliases("CharacteristicExponentRepresentation")]
    [ColumnNames("EXPONENT")]
    public string EXPONENT { get; set; }

    [Aliases("CharacteristicPlaceHolder")]
    [ColumnNames("TEMPLATE")]
    public string TEMPLATE { get; set; }

    [Aliases("CharacteristicIsSigned")]
    [ColumnNames("WITH_SIGN")]
    public char WITH_SIGN { get; set; }

    [Aliases("CharacteristicUnit")]
    [ColumnNames("UNIT_OF_MEASUREMENT")]
    public string UNIT_OF_MEASUREMENT { get; set; }

    [Aliases("CharacteristicUnitIso")]
    [ColumnNames("UNIT_OF_MEASUREMENT_ISO")]
    public string UNIT_OF_MEASUREMENT_ISO { get; set; }

    [Aliases("CharacteristicCurrency")]
    [ColumnNames("CURRENCY")]
    public string CURRENCY { get; set; }

    [Aliases("CharacteristicCurrencyIso")]
    [ColumnNames("CURRENCY_ISO")]
    public string CURRENCY_ISO { get; set; }

    [Aliases("CharacteristicStatus")]
    [ColumnNames("STATUS")]
    public char STATUS { get; set; }

    [Aliases("CharacteristicGroup")]
    [ColumnNames("CHARACT_GROUP")]
    public string CHARACT_GROUP { get; set; }

    [Aliases("CharacteristicIsMultiple")]
    [ColumnNames("VALUE_ASSIGNMENT")]
    public char VALUE_ASSIGNMENT { get; set; }

    [Aliases("CharacteristicHasEntry")]
    [ColumnNames("NO_ENTRY")]
    public char NO_ENTRY { get; set; }

    [Aliases("CharacteristicIsDisplayed")]
    [ColumnNames("NO_DISPLAY")]
    public char NO_DISPLAY { get; set; }

    [Aliases("CharacteristicRequired")]
    [ColumnNames("ENTRY_REQUIRED")]
    public char ENTRY_REQUIRED { get; set; }

    [Aliases("CharacteristicHasInterval")]
    [ColumnNames("INTERVAL_ALLOWED")]
    public char INTERVAL_ALLOWED { get; set; }

    [Aliases("CharacteristicTemplateCode")]
    [ColumnNames("SHOW_TEMPLATE")]
    public char SHOW_TEMPLATE { get; set; }

    [Aliases("CharacteristicDisplayValues")]
    [ColumnNames("DISPLAY_VALUES")]
    public char DISPLAY_VALUES { get; set; }

    [Aliases("CharacteristicAdditionalCodes")]
    [ColumnNames("ADDITIONAL_VALUES")]
    public char ADDITIONAL_VALUES { get; set; }

    [Aliases("CharacteristicDocNumber")]
    [ColumnNames("DOCUMENT_NO")]
    public string DOCUMENT_NO { get; set; }

    [Aliases("CharacteristicDocType")]
    [ColumnNames("DOCUMENT_TYPE")]
    public string DOCUMENT_TYPE { get; set; }

    [Aliases("CharacteristicDocPartVersion")]
    [ColumnNames("DOCUMENT_PART")]
    public string DOCUMENT_PART { get; set; }

    [Aliases("CharacteristicDocVersion")]
    [ColumnNames("DOCUMENT_VERSION")]
    public string DOCUMENT_VERSION { get; set; }

    [Aliases("CharacteristicCheckTbl")]
    [ColumnNames("CHECK_TABLE")]
    public string CHECK_TABLE { get; set; }

    [Aliases("CharacteristicCheckFunction")]
    [ColumnNames("CHECK_FUNCTION")]
    public string CHECK_FUNCTION { get; set; }

    [Aliases("CharacteristicCentre")]
    [ColumnNames("PLANT")]
    public string PLANT { get; set; }

    [Aliases("CharacteristicSet")]
    [ColumnNames("SELECTED_SET")]
    public string SELECTED_SET { get; set; }

    [Aliases("CharacteristicClassId")]
    [ColumnNames("ADT_CLASS")]
    public string ADT_CLASS { get; set; }

    [Aliases("CharacteristicClassType")]
    [ColumnNames("ADT_CLASS_TYPE")]
    public string ADT_CLASS_TYPE { get; set; }

    [Aliases("CharacteristicHasAggregate")]
    [ColumnNames("AGGREGATING")]
    public char AGGREGATING { get; set; }

    [Aliases("CharacteristicHasBalance")]
    [ColumnNames("BALANCING")]
    public char BALANCING { get; set; }

    [Aliases("CharacteristicRequireConfig")]
    [ColumnNames("INPUT_REQUESTED_CONF")]
    public char INPUT_REQUESTED_CONF { get; set; }

    [Aliases("CharacteristicAuthGroup")]
    [ColumnNames("AUTHORITY_GROUP")]
    public char AUTHORITY_GROUP { get; set; }

    [Aliases("CharacteristicHasFormat")]
    [ColumnNames("UNFORMATED")]
    public char UNFORMATED { get; set; }
}

public interface ICharacteristic
{
    
}
