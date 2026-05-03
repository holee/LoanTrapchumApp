namespace Infrastructures.Utils;

public enum FilterOp
{
    // ===== Comparison =====
    Equal,                  // =
    NotEqual,               // <>
    GreaterThan,            // >
    GreaterThanOrEqual,     // >=
    LessThan,               // <
    LessThanOrEqual,        // <=

    // ===== String =====
    Contains,               // LIKE %x%
    NotContains,
    StartsWith,             // LIKE x%
    EndsWith,               // LIKE %x
    Like,                   // custom LIKE pattern
    NotLike,

    // ===== Null =====
    IsNull,                 // IS NULL
    IsNotNull,              // IS NOT NULL

    // ===== Range =====
    Between,                // BETWEEN a AND b
    NotBetween,

    // ===== Collection =====
    In,                     // IN (...)
    NotIn,                  // NOT IN (...)

    // ===== Boolean =====
    True,                   // = 1
    False,                  // = 0

    // ===== Advanced =====
    Exists,                 // EXISTS (subquery)
    NotExists
}
