import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { GoogleMapHomeComponent } from './google-map-home.component';

describe('GoogleMapHomeComponent', () => {
  let component: GoogleMapHomeComponent;
  let fixture: ComponentFixture<GoogleMapHomeComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ GoogleMapHomeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(GoogleMapHomeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
