export default class PagingParams {
  pageNumber: number
  pageSize: number

  constructor(pageNumber: number = 1, pageSize: number = 2) {
    this.pageNumber = pageNumber
    this.pageSize = pageSize
  }
}
