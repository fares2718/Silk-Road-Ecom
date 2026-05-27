import { Product } from "./product.model"

export interface Page {
  pageNumber: number
  pageSize: number
  totalCount: number
  data: Product[]
}

