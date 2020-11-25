import { DatePipe, formatDate } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';


@Component({
  selector: 'app-line-chart',
  templateUrl: './line-chart.component.html',
  styleUrls: ['./line-chart.component.css']
})
export class LineChartComponent implements OnInit {

  options: any;
  updateOptions: any;
  @Input() data: [];
  timer: any;

  constructor(private datepipe: DatePipe) { }

  ngOnInit(): void {


    //initial chart:
    this.options = {
      title: {
        text: 'History Data',
        textStyle: {
          color: 'white'
        }
      }, grid: {
        backgroundColor: 'rgba(254, 254, 254, 0.48)',
        show: true
      },
      tooltip: {
        trigger: 'axis'
      },
      xAxis: {
        type: 'time',
        splitLine: {
          show: false
        },
        splitNumber: 10,

        axisTick: {
          alignWithLabel: true,
          show: true
        },
        axisLine: {
          show: true,
            lineStyle: {
              color: "white"
            }
        },
        axisLabel: {
          color: 'white',
          showMaxLabel: true
          ,
          formatter: (function (value: number) {

            return formatDate(value, 'HH:mm dd/MM/yyyy', 'vi').toString();
          })
        }
      },
      yAxis: {
        splitLine: {
          show: false
        }, min: 0, max: 500, axisLine: {
          lineStyle: {
            color: 'white'
          }
        }
      },
      toolbox: {
        show: true,
        showTitle: false,
        left: 'center', // hide the default text so they don't overlap each other
        feature: {
          saveAsImage: {
            show: true,
            title: 'Save As Image',
            backgroundColor: '#95989B'
          }, restore: {
            show: true,
            title: 'restore'
          }, magicType: {
            type: ['line', 'bar'],
            title: {
              'line': 'line',
              'bar': 'bar'
            }
          }

        },
        tooltip: { // same as option.tooltip
          show: true,
          formatter: function (param) {
            return '<div>' + param.title + '</div>'; // user-defined DOM structure
          },
          backgroundColor: '#222',
          textStyle: {
            fontSize: 12,
          },
          extraCssText: 'box-shadow: 0 0 3px rgba(0, 0, 0, 0.3);' // user-defined CSS styles
        }
      },
      dataZoom: [{
        startValue: '2020-06-01'
      }, {
        type: 'slider'
      }, {
        dataBackground: {
          lineStyle: {
            color: 'rgba(203, 248, 208, 1)'
          },
          areaStyle: {
            color: 'rgba(227, 48, 48, 1)'
          }
        },
        textStyle: {
          color: 'rgba(248, 243, 243, 1)'
        }
      }],
      visualMap: {
        top: 10,
        right: 10,
        textStyle: {
          color: 'white'
        },
        pieces: [{
          gt: 0,
          lte: 50,
          color: '#23DA27'
        }, {
          gt: 50,
          lte: 100,
          color: '#FFFD38'
        }, {
          gt: 100,
          lte: 150,
          color: '#FC7D23'
        }, {
          gt: 150,
          lte: 200,
          color: '#FC0D1B'
        }, {
          gt: 200,
          lte: 300,
          color: '#97084D'
        }, {
          gt: 300,
          color: '#7C0425'
        }],
        outOfRange: {
          color: '#999'
        }
      },
      series: {
        name: 'AQI',
        type: 'line',
        showSymbol: false,
        hoverAnimation: true,
        data: this.data,
        markLine: {
          silent: true,
          data: [{
            yAxis: 50
          }, {
            yAxis: 100
          }, {
            yAxis: 150
          }, {
            yAxis: 200
          }, {
            yAxis: 300
          }]
        }
      }
    }

    this.timer = setInterval(() => {
      this.updateOptions = {
        series: [{
          data: this.data
        }]
      }
    }, 1000)

    // // Mock dynamic data:
    // this.timer = setInterval(() => {
    //   for (let i = 0; i < 5; i++) {
    //     this.data.shift();
    //     this.data.push(this.randomData());
    //   }

    //   // update series data:
    //   this.updateOptions = {
    //     series: [{
    //       data: this.data
    //     }]
    //   };
    // }, 1000);

  }

  ngOnDestroy() {

    clearInterval(this.timer);
  }



}
