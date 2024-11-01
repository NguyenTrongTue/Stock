import Enums from "@/resources/enumeration";
export const columns = [
    {
        field: "stock_code",
        headerName: "Mã",
        type: Enums.EnumColumnType.Text,
        textAlign: 'left',
        width: 100,
        minWidth: 100,
        isColor: true,
    },
    {
        field: "matched_price",
        headerName: "Khớp",
        type: Enums.EnumColumnType.Decimal,
        textAlign: 'right',
        width: 60,
        minWidth: 60,
        isColor: true,
    },
    {
        field: "change_price",
        headerName: "%",
        type: Enums.EnumColumnType.Percent,
        textAlign: 'right',
        width: 60,
        minWidth: 60,
        isColor: true,
    },
    {
        field: "total_volume",
        headerName: "Tổng KL",
        type: Enums.EnumColumnType.Number,
        textAlign: 'right',
        width: 80,
        minWidth: 80,
    },
];
