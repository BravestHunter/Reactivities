import { useCallback } from 'react'
import { useDropzone } from 'react-dropzone'
import { Header, Icon } from 'semantic-ui-react'

interface Props {
  setFiles: (files: any) => void
}

export default function PhotoDropzoneWidget(props: Props) {
  const { setFiles } = props

  const onDrop = useCallback(
    (acceptedFiles: any) => {
      setFiles(
        acceptedFiles.map((file: any) =>
          Object.assign(file, {
            preview: URL.createObjectURL(file),
          })
        )
      )
    },
    [setFiles]
  )
  const { getRootProps, getInputProps, isDragActive } = useDropzone({ onDrop })

  const dropzoneStyle = {
    border: 'dashed 3px #eee',
    borderColor: '#eee',
    borderRadius: '5px',
    paddingTop: '30px',
    textAlign: 'center' as 'center',
    height: 200,
  }

  const dropzoneActiveStyle = {
    borderColor: 'green',
  }

  return (
    <div
      {...getRootProps()}
      style={
        isDragActive
          ? { ...dropzoneStyle, ...dropzoneActiveStyle }
          : dropzoneStyle
      }
    >
      <input {...getInputProps()} />
      <Icon name="upload" size="huge" />
      <Header content="Drop image here" />
    </div>
  )
}
