import Enums from "@/resources/enumeration";

export const normalCommandColumns = [
    {
        field: "transaction_type",
        headerName: "Lệnh",
        type: Enums.EnumColumnType.Enum,
        enumName: 'TransactionType',
        textAlign: 'left',
        width: 50,
        minWidth: 100,
        isColor: true,
        funcCusColor: (value, keyColor = null) => {
            let key = (keyColor || keyColor === 0) ? keyColor : value;
            let result = 'white';
            switch (key) {
                case 0:
                    result = 'green';
                    break;
                case 1:
                    result = 'red';
                    break;
                default:
                    result = 'white';
                    break;
            }
            return result;
        },
    },
    {
        field: "stock_code",
        headerName: "Mã",
        type: Enums.EnumColumnType.Text,
        textAlign: 'right',
        width: 60,
        minWidth: 60,
    },
    {
        field: "created_at",
        headerName: "Thời gian",
        type: Enums.EnumColumnType.Time,
        textAlign: 'right',
        width: 120,
        minWidth: 60
    },
    {
        field: "order_price",
        headerName: "Giá đặt",
        type: Enums.EnumColumnType.Text,
        textAlign: 'right',
        width: 80,
        minWidth: 80,
    },
    {
        field: "matched_price",
        headerName: "Giá khớp",
        type: Enums.EnumColumnType.Text,
        textAlign: 'right',
        width: 80,
        minWidth: 80,
    },
    {
        field: "volume",
        headerName: "Khối lượng",
        type: Enums.EnumColumnType.Number,
        textAlign: 'right',
        width: 80,
        minWidth: 80,
    },
    {
        field: "status",
        headerName: "Trạng thái",
        type: Enums.EnumColumnType.Enum,
        enumName: 'TransactionStatus',
        textAlign: 'right',
        width: 80,
        minWidth: 80,
        functionCustomColorTD: (status) => {
            switch (status) {
                case 0:
                    return 'yellow';
                case 1:
                    return 'green';

            }
        }
    },
];


export const dealColumns = [
    {
        field: "stock_code",
        headerName: "Mã",
        type: Enums.EnumColumnType.Text,
        textAlign: 'right',
        width: 60,
        minWidth: 60,
    },
    {
        field: "total_volume",
        headerName: "Khối lượng mở",
        type: Enums.EnumColumnType.Text,
        textAlign: 'right',
        width: 120,
        minWidth: 60,
    },
    {
        field: "total_tradeable_volume",
        headerName: "Được giao dịch",
        type: Enums.EnumColumnType.Text,
        textAlign: 'right',
        width: 80,
        minWidth: 80,
    },
    {
        field: "cost_value",
        headerName: "Giá hòa vốn",
        type: Enums.EnumColumnType.Text,
        textAlign: 'right',
        width: 80,
        minWidth: 80,
    },
    {
        field: "current_price",
        headerName: "Giá hiện tại",
        type: Enums.EnumColumnType.Text,
        textAlign: 'right',
        width: 80,
        minWidth: 80,
        isColor: true,
    },
    {
        field: "market_value",
        headerName: "Giá trị hiện tại",
        type: Enums.EnumColumnType.Number,
        textAlign: 'right',
        width: 80,
        minWidth: 80,
    },

    {
        field: "profit_loss",
        headerName: "Lãi chưa chốt",
        type: Enums.EnumColumnType.Number,
        textAlign: 'right',
        width: 80,
        minWidth: 80,
        isColor: true,
    },
    {
        field: "profit_loss_by_percent",
        headerName: "% lãi",
        type: Enums.EnumColumnType.Percent,
        textAlign: 'right',
        width: 80,
        minWidth: 80,
    },
];
