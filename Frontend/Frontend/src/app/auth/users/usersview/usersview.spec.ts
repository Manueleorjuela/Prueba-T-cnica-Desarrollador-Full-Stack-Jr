import { ComponentFixture, TestBed } from '@angular/core/testing';

import { Usersview } from './usersview';

describe('Usersview', () => {
  let component: Usersview;
  let fixture: ComponentFixture<Usersview>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Usersview]
    })
    .compileComponents();

    fixture = TestBed.createComponent(Usersview);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
