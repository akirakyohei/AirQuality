import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DetailMarkerComponent } from './detail-marker.component';

describe('DetailMarkerComponent', () => {
  let component: DetailMarkerComponent;
  let fixture: ComponentFixture<DetailMarkerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetailMarkerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DetailMarkerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
