<template>
    <div>
        <apexchart width="500" type="donut" :options="chartOptions" :series="series"></apexchart>
    </div>
</template>

<script>
export default {
    /**
     * Dữ liệu cho biểu đồ doughnut.
     *
     * Gồm 2 phần: series và chartOptions.
     * - Series: Là dữ liệu cho các phần của biểu đồ.
     *   - data: Là mảng số nguyên, mỗi phần tử là phần trăm của phần tương ứng.
     * - ChartOptions: Là các tùy chọn cho biểu đồ.
     *   - chart: Loại biểu đồ.
     *   - labels: Nhãn cho các phần của biểu đồ.
     *   - responsive: Tùy chọn cho màn hình khác nhau.
     *       - breakpoint: Kích thước màn hình.
     *       - options: Các tùy chọn cho màn hình.
     *           - chart: Tùy chọn cho bi  u đồ.
     *           - legend: Tùy chọn cho legend.
     */
    data() {
        return {
            series: [10.8, 15.3],
            chartOptions: {
                chart: {
                    type: 'donut', // Đặt loại biểu đồ là 'donut'
                },
                labels: ['Tiền', 'Cổ phiếu'], // Nhãn cho các phần của biểu đồ
                legend: {
                    formatter: function (val, opts) {
                        return `<span class="legend-text-wrapper"><span class="legend-text">${val}</span><span>` + " " + opts.w.globals.series[opts.seriesIndex] + ' triệu';
                    },
                    fontSize: '16px', // Tùy chỉnh kích thước font chữ
                    fontWeight: 600, // Độ dày font chữ                    
                    labels: {
                        colors: ['#fff', '#fff'], // Đặt màu sắc cho từng mục trong legend
                    },
                    marginLeft: '10px',
                },
                tooltip: {
                    enabled: true,
                    y: {
                        // Custom tooltip value when hovering
                        formatter: function (val, opts) {
                            return val + ' triệu'; // Show 'triệu' after the value when hover
                        }
                    }
                },
                responsive: [
                    {
                        breakpoint: 480,
                        options: {
                            chart: {
                                width: 200, // Điều chỉnh chiều rộng cho màn hình nhỏ hơn 480px
                            },
                            legend: {
                                position: 'bottom', // Vị trí legend cho màn hình nhỏ
                            },
                        },
                    },
                ],
                dataLabels: {
                    enabled: true,
                    formatter: function (val) {
                        return val.toFixed(2) + "%"
                    },

                }
            }
        };
    },
};
</script>