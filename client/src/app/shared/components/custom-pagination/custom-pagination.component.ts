import { Component, input, output } from '@angular/core';
import { PaginationComponent } from "ngx-bootstrap/pagination";

@Component({
  selector: 'app-custom-pagination',
  standalone: true,
  imports: [PaginationComponent],
  templateUrl: './custom-pagination.component.html',
  styleUrl: './custom-pagination.component.scss'
})
export class CustomPaginationComponent {
totalCount = input.required<number>();
pageSize = input.required<number>();
pageChanged = output<number>();


onPageChanged(pageNum:number){
  this.pageChanged.emit(pageNum);
}
}
