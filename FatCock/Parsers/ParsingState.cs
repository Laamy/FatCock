public enum ParsingState
{
    Idle, // idle (Looking for something to do)
    Import, // import (Looking for a header)
    ImportAs,
    ImportAsIdentifier,
    ImportAsIdentifierFrom,
    Class,
    InsideClass,
    InsideClassFunction
}