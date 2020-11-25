import { Component, OnInit, Input, ViewChild, ElementRef, Renderer2 } from '@angular/core';


@Component({
  selector: 'app-linear-gauge',
  templateUrl: './linear-gauge.component.html',
  styleUrls: ['./linear-gauge.component.css']
})
export class LinearGaugeComponent implements OnInit {

@ViewChild('linear_gauge',{static:true}) linear_gauge:ElementRef;
@Input() name='';
@Input() unit='';
@Input() value=Math.floor((Math.random() * 100) + 1);
@Input() min=0;
@Input() max=100;
@Input() thresholds:Thresholds[];
@Input() color='#23DA27';

  constructor(private render:Renderer2) {
  }


  ngOnInit(): void {
    let ratio_bar:HTMLParagraphElement =this.render.createElement('div');
    ratio_bar.classList.add('ratio-bar');
    if(this.thresholds!==undefined){
    this.thresholds.forEach(element => {
      if(this.value>=Number.parseFloat(element.value)){
        ratio_bar.style.backgroundColor=element.color;

      }
    });}else{
      ratio_bar.style.backgroundColor=this.color;
    }
    let ratio_bar_bg:HTMLParagraphElement=this.render.createElement('div');
    ratio_bar_bg.classList.add('ratio-bar-bg');

    let widthRatioBarBg=Number.parseFloat(ratio_bar_bg.style.width);
    let widthRatioBar=this.value/(this.max-this.min);
    ratio_bar.style.width='calc(100%*'+widthRatioBar.toString()+')';

    console.log(ratio_bar_bg.style.width)
    this.render.appendChild(this.linear_gauge.nativeElement,ratio_bar_bg);
    this.render.appendChild(this.linear_gauge.nativeElement,ratio_bar);


  }




}

 interface Thresholds{
 color:string;
 value:string;
}
