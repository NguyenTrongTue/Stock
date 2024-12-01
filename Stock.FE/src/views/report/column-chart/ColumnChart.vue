<template>
    <div>
        <apexchart type="bar" height="450" :options="chartOptions" :series="series"></apexchart>
    </div>
</template>

<script>
export default {
    props: {
        dataProps: {
            type: Object,
            default: []
        }
    },

    data() {
        return {
            series: [{ name: 'Chênh lệch', data: [] }],
            chartOptions: {
                chart: {
                    type: 'bar',
                    height: 450
                },
                dataLabels: {
                    enabled: true,

                    style: {
                        fontSize: '12px',
                        colors: ["white"]
                    }
                },
                responsive: [
                    {
                        breakpoint: 480,
                        options: {
                            legend: {
                                position: 'bottom',
                                offsetX: -10,
                                offsetY: 0
                            }
                        }
                    }
                ],
                xaxis: {
                    categories: [],
                    labels: {
                        style: {
                            colors: ['#fff'],
                            fontSize: '14px',
                            fontWeight: 600
                        }
                    }
                },
                yaxis: {
                    labels: {
                        // formatter: function (value) {
                        //     return value / 1000000 + " M"; // Thêm text sau giá trị trục Y
                        // },
                        style: {
                            colors: ['#85868a'], // Đổi màu chữ nhãn trục Y
                            fontSize: '14px', // Thay đổi kích thước font chữ
                            fontWeight: 600 // Độ dày của font chữ
                        }
                    }
                },
                fill: {
                    opacity: 1
                },
                legend: {
                    position: 'right',
                    offsetX: 0,
                    offsetY: 50
                }
            }
        };
    },
    watch: {
        dataProps: {

            handler(newValue) {
                const me = this;
                if (newValue) {
                    me.series[0].data = newValue.data;



                    this.chartOptions = {
                        ...this.chartOptions,
                        xaxis: {
                            ...this.chartOptions.xaxis,
                            categories: newValue.labels
                        }
                    };
                }
            },
            deep: true
        }
    }

};
</script>

<style scoped lang="scss"></style>
