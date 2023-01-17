import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AddInfoComponent } from './shared/add-info/add-info.component';
import { InfoListComponent } from './shared/info-list/info-list.component';

const routes: Routes = [
  {
    path: '',
    component: AddInfoComponent
  },
  {
    path: 'add-info',
    component: AddInfoComponent
  },
  {
    path: 'info-list',
    component: InfoListComponent
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
