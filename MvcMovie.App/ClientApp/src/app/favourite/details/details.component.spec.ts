import { ComponentFixture, TestBed } from '@angular/core/testing';

import { FavouriteDetailsComponent } from './details.component';

describe('FavouriteDetailsComponent', () => {
  let component: FavouriteDetailsComponent;
  let fixture: ComponentFixture<FavouriteDetailsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ FavouriteDetailsComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(FavouriteDetailsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
