export function sleep(delay: number) {
  return new Promise((resolve) => {
    setTimeout(resolve, delay)
  })
}

export function toURLSearchParams(params: object): URLSearchParams {
  const searchParams = new URLSearchParams()

  Object.entries(params).forEach(([key, value]) => {
    if (value !== undefined && value !== null) {
      if (value instanceof Date) {
        searchParams.append(key, value.toISOString())
      } else {
        searchParams.append(key, String(value))
      }
    }
  })

  return searchParams
}
